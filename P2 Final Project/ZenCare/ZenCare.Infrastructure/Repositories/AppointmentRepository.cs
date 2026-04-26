using Microsoft.EntityFrameworkCore;
using ZenCare.Application.Interfaces;
using ZenCare.Domain.Entities;
using ZenCare.Infrastructure.Data;

namespace ZenCare.Infrastructure.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ZenCareDbContext context) : base(context) { }

    public async Task<Appointment?> GetWithDetailsAsync(Guid id) =>
        await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Specialist)
            .Include(a => a.Therapy)
            .Include(a => a.Notes)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<IEnumerable<Appointment>> GetByPatientAsync(Guid patientId) =>
        await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Specialist)
            .Include(a => a.Therapy)
            .Where(a => a.PatientId == patientId && a.IsActive)
            .OrderByDescending(a => a.ScheduledAt)
            .ToListAsync();

    public async Task<IEnumerable<Appointment>> GetBySpecialistAsync(
        Guid specialistId) =>
        await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Therapy)
            .Where(a => a.SpecialistId == specialistId && a.IsActive)
            .OrderBy(a => a.ScheduledAt)
            .ToListAsync();

    public async Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date)
    {
        // Convertir a UTC para PostgreSQL
        var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        var start = utcDate.Date;
        var end = start.AddDays(1);

        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Specialist)
            .Include(a => a.Therapy)
            .Where(a => a.ScheduledAt >= start
                     && a.ScheduledAt < end
                     && a.IsActive)
            .OrderBy(a => a.ScheduledAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetSpecialistScheduleAsync(
        Guid specialistId, DateTime start, DateTime end)
    {
        // Convertir a UTC para PostgreSQL
        var utcStart = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        var utcEnd = DateTime.SpecifyKind(end, DateTimeKind.Utc);

        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Therapy)
            .Where(a => a.SpecialistId == specialistId
                     && a.ScheduledAt >= utcStart
                     && a.ScheduledAt <= utcEnd
                     && a.IsActive)
            .OrderBy(a => a.ScheduledAt)
            .ToListAsync();
    }

    public async Task<bool> HasScheduleConflictAsync(
        Guid specialistId, DateTime scheduledAt, Guid? excludeId = null)
    {
        // Convertir a UTC para PostgreSQL
        var utcScheduledAt = DateTime.SpecifyKind(scheduledAt, DateTimeKind.Utc);
        var from = utcScheduledAt.AddMinutes(-60);
        var to = utcScheduledAt.AddMinutes(60);

        var q = _context.Appointments.Where(a =>
            a.SpecialistId == specialistId &&
            a.ScheduledAt >= from &&
            a.ScheduledAt <= to &&
            a.Status != AppointmentStatus.Cancelled &&
            a.IsActive);

        if (excludeId.HasValue)
            q = q.Where(a => a.Id != excludeId.Value);

        return await q.AnyAsync();
    }
}