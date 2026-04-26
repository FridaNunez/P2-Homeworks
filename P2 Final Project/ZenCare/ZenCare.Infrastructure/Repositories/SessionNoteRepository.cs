using Microsoft.EntityFrameworkCore;
using ZenCare.Application.Interfaces;
using ZenCare.Domain.Entities;
using ZenCare.Infrastructure.Data;

namespace ZenCare.Infrastructure.Repositories;

public class SessionNoteRepository : Repository<SessionNote>, ISessionNoteRepository
{
    public SessionNoteRepository(ZenCareDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SessionNote>> GetByAppointmentAsync(Guid appointmentId) =>
        await _context.SessionNotes
            .Where(n => n.AppointmentId == appointmentId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
}