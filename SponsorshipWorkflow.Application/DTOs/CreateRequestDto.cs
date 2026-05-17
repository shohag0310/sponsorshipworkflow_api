namespace SponsorshipWorkflow.Application.DTOs;

public class CreateRequestDto
{
    public string Title { get; set; } = default!;

    public string Department { get; set; } = default!;

    public Guid SponsorshipTypeId { get; set; }

    public string EventName { get; set; } = default!;

    public DateTime EventDate { get; set; }

    public decimal RequestedAmount { get; set; }

    public string Purpose { get; set; } = default!;

    public string? ExpectedBusinessBenefit { get; set; }

    public string? Remarks { get; set; }
}

