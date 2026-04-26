using ZenCare.Domain.Entities;

namespace ZenCare.Application.Interfaces;

public interface ITherapyRepository : IRepository<Therapy>
{
    Task<IEnumerable<Therapy>> GetActiveAsync();
    Task<IEnumerable<Therapy>> GetByCategoryAsync(string category);
}