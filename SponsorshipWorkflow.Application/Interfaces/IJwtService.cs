using SponsorshipWorkflow.Domain.Entities;

namespace SponsorshipWorkflow.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}