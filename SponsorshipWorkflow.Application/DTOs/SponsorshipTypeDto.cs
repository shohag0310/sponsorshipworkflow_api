namespace SponsorshipWorkflow.Application.DTOs;

public class CreateSponsorshipTypeDto
{
    public string Name { get; set; } = default!;
}

public class UpdateSponsorshipTypeDto : CreateSponsorshipTypeDto
{

}