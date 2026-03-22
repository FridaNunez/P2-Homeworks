using Pet_Rescue.Models.Entities;

namespace Pet_Rescue.Domain.Interfaces

{

    public interface IPetRepository
    {
        Task<IEnumerable<Pet>> GetAllAsync();


        Task<Pet> GetByIdAsync(int id);


        Task CreateAsync(Pet pet);


        Task DeleteAsync(int id);

    }

}