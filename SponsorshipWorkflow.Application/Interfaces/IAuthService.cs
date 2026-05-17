using SponsorshipWorkflow.Application.DTOs;

namespace SponsorshipWorkflow.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}