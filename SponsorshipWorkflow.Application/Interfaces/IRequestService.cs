using SponsorshipWorkflow.Application.DTOs;
using SponsorshipWorkflow.Domain.Entities;

namespace SponsorshipWorkflow.Application.Interfaces;

public interface IRequestService
{
    Task<SponsorshipRequest> CreateAsync(CreateRequestDto dto, Guid requestorId);
    Task<List<SponsorshipRequest>> GetAllAsync(Guid currentUserId, string role);
    Task<SponsorshipRequest?> GetByIdAsync(Guid id);
    Task<SponsorshipRequest> UpdateAsync(Guid id, UpdateRequestDto dto, Guid requestorId);
    Task<List<ApprovalHistory>> GetHistoryAsync(Guid requestId);
    Task<List<ApprovalHistory>> GetAllHistoryAsync();
}