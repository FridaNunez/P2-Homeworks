using ZenCare.Domain.Entities;

namespace ZenCare.Application.Interfaces;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<Appointment?> GetWithDetailsAsync(Guid id);
    Task<IEnumerable<Appointment>> GetByPatientAsync(Guid patientId);
    Task<IEnumerable<Appointment>> GetBySpecialistAsync(Guid specialistId);
    Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date);
    Task<IEnumerable<Appointment>> GetSpecialistScheduleAsync(
        Guid specialistId, DateTime start, DateTime end);
    Task<bool> HasScheduleConflictAsync(
        Guid specialistId, DateTime scheduledAt, Guid? excludeId = null);
}