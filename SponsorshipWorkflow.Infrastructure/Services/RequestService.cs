using Microsoft.EntityFrameworkCore;
using SponsorshipWorkflow.Application.DTOs;
using SponsorshipWorkflow.Application.Interfaces;
using SponsorshipWorkflow.Domain.Entities;
using SponsorshipWorkflow.Domain.Enums;
using SponsorshipWorkflow.Infrastructure.Persistence;

namespace SponsorshipWorkflow.Infrastructure.Services;

public class RequestService : IRequestService
{
    private readonly AppDbContext _context;

    public RequestService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SponsorshipRequest> CreateAsync(CreateRequestDto dto, Guid requestorId)
    {
        var request = new SponsorshipRequest
        {
            Title = dto.Title,
            Department = dto.Department,
            SponsorshipTypeId = dto.SponsorshipTypeId,
            EventName = dto.EventName,
            EventDate = DateTime.SpecifyKind(dto.EventDate, DateTimeKind.Utc),
            RequestedAmount = dto.RequestedAmount,
            Purpose = dto.Purpose,
            ExpectedBusinessBenefit = dto.ExpectedBusinessBenefit,
            Remarks = dto.Remarks,
            RequestorId = requestorId,
            Status = RequestStatus.Draft
        };

        await _context.SponsorshipRequests.AddAsync(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<List<SponsorshipRequest>> GetAllAsync(Guid currentUserId, string role)
    {
        IQueryable<SponsorshipRequest> query = _context.SponsorshipRequests
            .Include(x => x.Requestor)
            .Include(x => x.SponsorshipType);

        if (Enum.TryParse<UserRole>(role, out var userRole))
        {
            query = userRole switch
            {
                UserRole.Requestor => query.Where(x => x.RequestorId == currentUserId),
                UserRole.Manager => query.Where(x => x.Status == RequestStatus.PendingManagerApproval),
                UserRole.FinanceAdmin => query.Where(x => x.Status == RequestStatus.PendingFinanceReview),
                _ => query
            };
        }

        return await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task<SponsorshipRequest?> GetByIdAsync(Guid id)
    {
        return await _context.SponsorshipRequests
            .Include(x => x.Requestor)
            .Include(x => x.SponsorshipType)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SponsorshipRequest> UpdateAsync(Guid id, UpdateRequestDto dto, Guid requestorId)
    {
        var request = await _context.SponsorshipRequests.FindAsync(id);
        if (request == null)
            throw new Exception("Request not found");

        if (request.RequestorId != requestorId)
            throw new Exception("You can only update your own requests");

        if (request.Status != RequestStatus.Draft)
            throw new Exception("Only draft requests can be updated");

        if (!string.IsNullOrEmpty(dto.Title))
            request.Title = dto.Title;
        if (!string.IsNullOrEmpty(dto.Department))
            request.Department = dto.Department;
        if (dto.SponsorshipTypeId.HasValue)
            request.SponsorshipTypeId = dto.SponsorshipTypeId.Value;
        if (!string.IsNullOrEmpty(dto.EventName))
            request.EventName = dto.EventName;
        if (dto.EventDate.HasValue)
            request.EventDate = DateTime.SpecifyKind(dto.EventDate.Value, DateTimeKind.Utc);
        if (dto.RequestedAmount.HasValue)
            request.RequestedAmount = dto.RequestedAmount.Value;
        if (!string.IsNullOrEmpty(dto.Purpose))
            request.Purpose = dto.Purpose;
        if (dto.ExpectedBusinessBenefit != null)
            request.ExpectedBusinessBenefit = dto.ExpectedBusinessBenefit;
        if (dto.Remarks != null)
            request.Remarks = dto.Remarks;

        request.UpdatedAt = DateTime.UtcNow;

        _context.SponsorshipRequests.Update(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<List<ApprovalHistory>> GetHistoryAsync(Guid requestId)
    {
        return await _context.ApprovalHistories
            .Include(x => x.ActionByUser)
            .Where(x => x.RequestId == requestId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<ApprovalHistory>> GetAllHistoryAsync()
    {
        return await _context.ApprovalHistories
            .Include(x => x.ActionByUser)
            .Include(x => x.Request)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}