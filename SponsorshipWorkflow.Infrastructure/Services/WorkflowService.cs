using Microsoft.EntityFrameworkCore;
using SponsorshipWorkflow.Application.Interfaces;
using SponsorshipWorkflow.Domain.Entities;
using SponsorshipWorkflow.Domain.Enums;
using SponsorshipWorkflow.Infrastructure.Persistence;

namespace SponsorshipWorkflow.Infrastructure.Services;

public class WorkflowService : IWorkflowService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public WorkflowService(AppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task SubmitAsync(Guid requestId, Guid currentUserId, string currentUserRole)
    {
        var request = await GetRequestForRequestorAction(requestId, currentUserId, currentUserRole);

        if (request.Status != RequestStatus.Draft)
        {
            throw new InvalidOperationException("Only draft requests can be submitted.");
        }

        await ChangeStatus(request, RequestStatus.PendingManagerApproval, "Submitted", null);
    }

    public async Task CancelAsync(Guid requestId, Guid currentUserId, string currentUserRole)
    {
        var request = await GetRequestForRequestorAction(requestId, currentUserId, currentUserRole);

        if (request.Status is not (RequestStatus.Draft or RequestStatus.PendingManagerApproval or RequestStatus.PendingFinanceReview))
        {
            throw new InvalidOperationException("Only draft or pending requests can be cancelled.");
        }

        await ChangeStatus(request, RequestStatus.Cancelled, "Cancelled", null);
    }

    public async Task ManagerApproveAsync(Guid requestId, string? remarks)
    {
        var request = await GetRequest(requestId);

        if (request.Status != RequestStatus.PendingManagerApproval)
        {
            throw new Exception("Invalid request status.");
        }

        await ChangeStatus(request, RequestStatus.PendingFinanceReview, "Manager Approved", remarks);
    }

    public async Task ManagerRejectAsync(Guid requestId, string? remarks)
    {
        var request = await GetRequest(requestId);

        if (request.Status != RequestStatus.PendingManagerApproval)
        {
            throw new Exception("Invalid request status.");
        }

        await ChangeStatus(request, RequestStatus.Rejected, "Manager Rejected", remarks);
    }

    public async Task FinanceApproveAsync(Guid requestId, string? remarks)
    {
        var request = await GetRequest(requestId);

        if (request.Status != RequestStatus.PendingFinanceReview)
        {
            throw new Exception("Invalid request status.");
        }

        await ChangeStatus(request, RequestStatus.Approved, "Finance Approved", remarks);
    }

    public async Task FinanceRejectAsync(Guid requestId, string? remarks)
    {
        var request = await GetRequest(requestId);

        if (request.Status != RequestStatus.PendingFinanceReview)
        {
            throw new Exception("Invalid request status.");
        }

        await ChangeStatus(request, RequestStatus.Rejected, "Finance Rejected", remarks);
    }

    private async Task<SponsorshipRequest> GetRequest(Guid id)
    {
        var request = await _context.SponsorshipRequests.FirstOrDefaultAsync(x => x.Id == id);

        if (request == null)
        {
            throw new KeyNotFoundException("Request not found.");
        }

        return request;
    }

    private async Task<SponsorshipRequest> GetRequestForRequestorAction(Guid requestId, Guid currentUserId, string currentUserRole)
    {
        if (!string.Equals(currentUserRole, UserRole.Requestor.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Only requestors can perform this action.");
        }

        var request = await GetRequest(requestId);

        if (request.RequestorId != currentUserId)
        {
            throw new UnauthorizedAccessException("You can only perform this action on your own request.");
        }

        return request;
    }

    private async Task ChangeStatus(SponsorshipRequest request, RequestStatus newStatus, string action, string? remarks)
    {
        var oldStatus = request.Status;

        request.Status = newStatus;
        request.UpdatedAt = DateTime.UtcNow;

        var history = new ApprovalHistory
        {
            RequestId = request.Id,
            ActionByUserId = _currentUserService.UserId,
            FromStatus = oldStatus,
            ToStatus = newStatus,
            Action = action,
            Remarks = remarks
        };

        await _context.ApprovalHistories.AddAsync(history);

        await _context.SaveChangesAsync();
    }
}
