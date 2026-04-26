using Microsoft.EntityFrameworkCore;
using ZenCare.Application.Interfaces;
using ZenCare.Domain.Entities;
using ZenCare.Infrastructure.Data;

namespace ZenCare.Infrastructure.Repositories;

public class TherapyRepository : Repository<Therapy>, ITherapyRepository
{
    public TherapyRepository(ZenCareDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Therapy>> GetActiveAsync() =>
        await _context.Therapies
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .ToListAsync();

    public async Task<IEnumerable<Therapy>> GetByCategoryAsync(string category) =>
        await _context.Therapies
            .Where(t => t.IsActive && t.Category == category)
            .ToListAsync();
}