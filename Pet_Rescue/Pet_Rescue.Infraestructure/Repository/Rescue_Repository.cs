using Microsoft.EntityFrameworkCore;
using Pet_Rescue.Context.Infraestructure;
using Pet_Rescue.Domain.Interfaces;
using Pet_Rescue.Models.Entities;

namespace Pet_Rescue.Infrastructure.Repository
{
    public class RescueRepository : IRescueRepository
    {
        private readonly Pet_RescueDbContext _context;

        public RescueRepository(Pet_RescueDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rescue>> GetAllAsync()
            => await _context.Rescue.ToListAsync();

        public async Task<Rescue> GetByIdAsync(int id)
            => await _context.Rescue.FindAsync(id);

        public async Task CreateAsync(Rescue rescue)
        {
            await _context.Rescue.AddAsync(rescue);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rescue = await _context.Rescue.FindAsync(id);
            if (rescue != null)
            {
                _context.Rescue.Remove(rescue);
                await _context.SaveChangesAsync();
            }
        }
    }
}