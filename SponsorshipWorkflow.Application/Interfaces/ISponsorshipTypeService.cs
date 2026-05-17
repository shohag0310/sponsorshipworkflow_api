using SponsorshipWorkflow.Domain.Entities;

namespace SponsorshipWorkflow.Application.Interfaces;

public interface ISponsorshipTypeService
{
    Task<List<SponsorshipType>> GetAllAsync();
    Task<SponsorshipType?> GetByIdAsync(Guid id);
    Task<SponsorshipType> CreateAsync(string name );
    Task<SponsorshipType> UpdateAsync(Guid id, string name);
    Task DeleteAsync(Guid id);
}