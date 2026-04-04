using Microsoft.EntityFrameworkCore;
using SkillNet.Models.Entities;

namespace SkillNet.Repositories.Contexts
{
    public class SkillNet_Context : DbContext
    {
        // Constructor para el funcionamiento normal
        public SkillNet_Context(DbContextOptions<SkillNet_Context> options) : base(options) { }

        // Constructor vacío para que las migraciones no fallen
        public SkillNet_Context() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Cambia esto por tu cadena de conexión real si es necesario
                optionsBuilder.UseNpgsql("Host=localhost; Database=SkillNet_DB; Username=postgres; Password=090212");
            }
        }

        public DbSet<User> Users { get; set; }
    }
}