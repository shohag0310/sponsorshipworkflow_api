using Microsoft.EntityFrameworkCore;
using SponsorshipWorkflow.Application.Interfaces;
using SponsorshipWorkflow.Infrastructure.Persistence;
using SponsorshipWorkflow.Domain.Entities;

namespace SponsorshipWorkflow.Infrastructure.Services;

public class SponsorshipTypeService : ISponsorshipTypeService
{
    private readonly AppDbContext _context;

    public SponsorshipTypeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SponsorshipType>> GetAllAsync()
    {
        return await _context.SponsorshipTypes.ToListAsync();
    }

    public async Task<SponsorshipType?> GetByIdAsync(Guid id)
    {
        return await _context.SponsorshipTypes.FindAsync(id);
    }

    public async Task<SponsorshipType> CreateAsync(string name)
    {
        var sponsorshipType = new SponsorshipType
        {
            Name = name,
        };

        _context.SponsorshipTypes.Add(sponsorshipType);
        await _context.SaveChangesAsync();
        return sponsorshipType;
    }

    public async Task<SponsorshipType> UpdateAsync(Guid id, string name)
    {
        var sponsorshipType = await _context.SponsorshipTypes.FindAsync(id);
        if (sponsorshipType == null)
            throw new Exception("Sponsorship type not found");

        sponsorshipType.Name = name;
        sponsorshipType.UpdatedAt = DateTime.UtcNow;
        
        _context.SponsorshipTypes.Update(sponsorshipType);
        await _context.SaveChangesAsync();
        return sponsorshipType;
    }

    public async Task DeleteAsync(Guid id)
    {
        var sponsorshipType = await _context.SponsorshipTypes.FindAsync(id);
        if (sponsorshipType == null)
            throw new Exception("Sponsorship type not found");

        _context.SponsorshipTypes.Remove(sponsorshipType);
        await _context.SaveChangesAsync();
    }
}