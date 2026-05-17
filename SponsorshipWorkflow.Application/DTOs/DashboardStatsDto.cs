namespace SponsorshipWorkflow.Application.DTOs;

public class DashboardStatsDto
{
    public int TotalRequests { get; set; }
    public int Pending { get; set; }
    public int Approved { get; set; }
    public int Rejected { get; set; }
}
