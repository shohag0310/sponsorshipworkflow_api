namespace SponsorshipWorkflow.Application.Interfaces;

public interface IWorkflowService
{
    Task SubmitAsync(Guid requestId, Guid currentUserId, string currentUserRole);

    Task CancelAsync(Guid requestId, Guid currentUserId, string currentUserRole);

    Task ManagerApproveAsync(Guid requestId, string? remarks);

    Task ManagerRejectAsync(Guid requestId, string? remarks);

    Task FinanceApproveAsync(Guid requestId, string? remarks);

    Task FinanceRejectAsync(Guid requestId, string? remarks);
}
