using Microsoft.EntityFrameworkCore;
using ZenCare.Application.Interfaces;
using ZenCare.Domain.Entities;
using ZenCare.Infrastructure.Data;

namespace ZenCare.Infrastructure.Repositories;

public class SpecialistRepository : Repository<Specialist>, ISpecialistRepository
{
    public SpecialistRepository(ZenCareDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Specialist>> GetActiveAsync() =>
        await _context.Specialists
            .Where(s => s.IsActive)
            .OrderBy(s => s.LastName)
            .ToListAsync();

    public async Task<bool> LicenseExistsAsync(string licenseNumber, Guid? excludeId = null)
    {
        var query = _context.Specialists.Where(s => s.LicenseNumber == licenseNumber);
        if (excludeId.HasValue)
            query = query.Where(s => s.Id != excludeId.Value);
        return await query.AnyAsync();
    }
}