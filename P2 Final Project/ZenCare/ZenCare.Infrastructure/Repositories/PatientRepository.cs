using Microsoft.EntityFrameworkCore;
using ZenCare.Application.Interfaces;
using ZenCare.Domain.Entities;
using ZenCare.Infrastructure.Data;

namespace ZenCare.Infrastructure.Repositories;

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(ZenCareDbContext context) : base(context)
    {
    }

    public async Task<Patient?> GetWithAppointmentsAsync(Guid id) =>
        await _context.Patients
            .Include(p => p.Appointments)
                .ThenInclude(a => a.Therapy)
            .Include(p => p.Appointments)
                .ThenInclude(a => a.Specialist)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Patient>> GetActiveAsync() =>
        await _context.Patients
            .Where(p => p.IsActive)
            .OrderBy(p => p.LastName)
            .ToListAsync();

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
    {
        var query = _context.Patients.Where(p => p.Email == email);
        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);
        return await query.AnyAsync();
    }
}