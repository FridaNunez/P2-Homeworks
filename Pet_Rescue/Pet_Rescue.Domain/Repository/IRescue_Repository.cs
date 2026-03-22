using Pet_Rescue.Models.Entities;

namespace Pet_Rescue.Domain.Interfaces
{
    public interface IRescueRepository
    {
        Task<IEnumerable<Rescue>> GetAllAsync();

        Task<Rescue> GetByIdAsync(int id);

        Task CreateAsync(Rescue rescue);

        Task DeleteAsync(int id);
    }
}