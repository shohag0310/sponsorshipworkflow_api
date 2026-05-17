using SponsorshipWorkflow.Domain.Enums;

namespace SponsorshipWorkflow.Domain.Entities;

public class ApprovalHistory : BaseEntity
{
    public Guid RequestId { get; set; }

    public SponsorshipRequest Request { get; set; } = default!;

    public Guid ActionByUserId { get; set; }

    public User ActionByUser { get; set; } = default!;

    public RequestStatus FromStatus { get; set; }

    public RequestStatus ToStatus { get; set; }

    public string Action { get; set; } = default!;

    public string? Remarks { get; set; }
}