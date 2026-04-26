using ZenCare.Domain.Entities;

namespace ZenCare.Application.Interfaces;

public interface ISpecialistRepository : IRepository<Specialist>
{
    Task<IEnumerable<Specialist>> GetActiveAsync();
    Task<bool> LicenseExistsAsync(string licenseNumber, Guid? excludeId = null);
}