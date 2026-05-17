namespace SponsorshipWorkflow.Application.DTOs;

public class UpdateRequestDto
{
    public string? Title { get; set; }
    public string? Department { get; set; }
    public Guid? SponsorshipTypeId { get; set; }
    public string? EventName { get; set; }
    public DateTime? EventDate { get; set; }
    public decimal? RequestedAmount { get; set; }
    public string? Purpose { get; set; }
    public string? ExpectedBusinessBenefit { get; set; }
    public string? Remarks { get; set; }
}