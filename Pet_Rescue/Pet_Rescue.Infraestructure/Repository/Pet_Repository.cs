using Microsoft.EntityFrameworkCore;
using Pet_Rescue.Domain.Interfaces;
using Pet_Rescue.Context.Infraestructure; 
using Pet_Rescue.Models.Entities;

namespace Pet_Rescue.Infrastructure.Repository
{
    public class PetRepository : IPetRepository
    {
        private readonly Pet_RescueDbContext _context;

        public PetRepository(Pet_RescueDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pet>> GetAllAsync()
            => await _context.Pet.ToListAsync();

        public async Task<Pet> GetByIdAsync(int id)
            => await _context.Pet.FindAsync(id);

        public async Task CreateAsync(Pet pet)
        {
            await _context.Pet.AddAsync(pet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pet = await _context.Pet.FindAsync(id);
            if (pet != null)
            {
                _context.Pet.Remove(pet);
                await _context.SaveChangesAsync();
            }
        }
    }
}