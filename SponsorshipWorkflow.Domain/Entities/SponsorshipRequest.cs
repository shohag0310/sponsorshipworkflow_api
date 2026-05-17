using SponsorshipWorkflow.Domain.Enums;

namespace SponsorshipWorkflow.Domain.Entities;

public class SponsorshipRequest : BaseEntity
{
    public string Title { get; set; } = default!;

    public string Department { get; set; } = default!;

    public string EventName { get; set; } = default!;

    public DateTime EventDate { get; set; }

    public decimal RequestedAmount { get; set; }

    public string Purpose { get; set; } = default!;

    public string? ExpectedBusinessBenefit { get; set; }

    public string? Remarks { get; set; }

    public Guid RequestorId { get; set; }

    public User Requestor { get; set; } = default!;

    public Guid SponsorshipTypeId { get; set; }

    public SponsorshipType SponsorshipType { get; set; } = default!;

    public RequestStatus Status { get; set; } = RequestStatus.Draft;
}