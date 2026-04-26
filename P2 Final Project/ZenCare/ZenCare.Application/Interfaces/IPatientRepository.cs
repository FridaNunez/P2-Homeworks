using ZenCare.Domain.Entities;

namespace ZenCare.Application.Interfaces;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetWithAppointmentsAsync(Guid id);
    Task<IEnumerable<Patient>> GetActiveAsync();
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
}