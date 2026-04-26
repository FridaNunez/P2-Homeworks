using ZenCare.Domain.Entities;

namespace ZenCare.Application.Interfaces;

public interface ISessionNoteRepository : IRepository<SessionNote>
{
    Task<IEnumerable<SessionNote>> GetByAppointmentAsync(Guid appointmentId);
}