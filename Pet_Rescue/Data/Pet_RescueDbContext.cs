using Microsoft.EntityFrameworkCore;
using Pet_Rescue.Models.Entities;

namespace Pet_Rescue.Data
{
    public class Pet_RescueDbContext : DbContext
    {
        public Pet_RescueDbContext(DbContextOptions<Pet_RescueDbContext> options)
          : base(options)
        {
        }

        public DbSet<Pet> Pet { get; set; }
        public DbSet<Rescue> Rescue { get; set; }
       
    }
}
