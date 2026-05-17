namespace SponsorshipWorkflow.Application.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Role { get; set; } = default!;
}