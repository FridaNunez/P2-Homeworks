using SkillNet.Models.Entities;

namespace SkillNet.Repositories.Contracts
{
    public interface IUser_Repository
    {
        Task<IEnumerable<User>> GetAllAsync();           // Leer todos
        Task<User?> GetByIdAsync(int id);                // Leer uno
        Task CreateAsync(User user);                     // Crear
        Task UpdateAsync(User user);                     // Actualizar
        Task<bool> DeleteAsync(int id);                  // Borrar
    }
}