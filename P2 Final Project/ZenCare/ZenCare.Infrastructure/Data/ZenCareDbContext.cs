using Microsoft.EntityFrameworkCore;
using ZenCare.Domain.Entities;

namespace ZenCare.Infrastructure.Data;

public class ZenCareDbContext : DbContext
{
    public ZenCareDbContext(DbContextOptions<ZenCareDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Specialist> Specialists => Set<Specialist>();
    public DbSet<Therapy> Therapies => Set<Therapy>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<SessionNote> SessionNotes => Set<SessionNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Patient ───────────────────────────────────────────────
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.MedicalNotes).HasMaxLength(2000);
            entity.Ignore(e => e.FullName);
        });

        // ── Specialist ────────────────────────────────────────────
        modelBuilder.Entity<Specialist>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.LicenseNumber).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Bio).HasMaxLength(1000);
            entity.Property(e => e.SpecialtiesStr).HasMaxLength(500);
            entity.Ignore(e => e.TherapySpecialties);
            entity.Ignore(e => e.FullName);
        });

        // ── Therapy ───────────────────────────────────────────────
        modelBuilder.Entity<Therapy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        // ── Appointment ───────────────────────────────────────────
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.ConsultationReason).HasMaxLength(500);
            entity.Property(e => e.SessionSummary).HasMaxLength(1000);

            entity.HasOne(a => a.Patient)
                  .WithMany(p => p.Appointments)
                  .HasForeignKey(a => a.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Specialist)
                  .WithMany(s => s.Appointments)
                  .HasForeignKey(a => a.SpecialistId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Therapy)
                  .WithMany(t => t.Appointments)
                  .HasForeignKey(a => a.TherapyId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── SessionNote ───────────────────────────────────────────
        modelBuilder.Entity<SessionNote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.RecordedBy).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Recommendations).HasMaxLength(1000);

            entity.HasOne(n => n.Appointment)
                  .WithMany(a => a.Notes)
                  .HasForeignKey(n => n.AppointmentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var therapy1Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");
        var therapy2Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002");
        var therapy3Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000003");
        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Therapy>().HasData(
            new
            {
                Id = therapy1Id,
                Name = "Relaxing Massage",
                Description = "Massage therapy to reduce stress and muscle tension.",
                DurationMinutes = 60,
                Price = 450.00m,
                Category = "Relaxation",
                CreatedAt = seedDate,
                IsActive = true
            },
            new
            {
                Id = therapy2Id,
                Name = "Acupuncture",
                Description = "Traditional Chinese medicine technique using needles "
                            + "at specific points.",
                DurationMinutes = 45,
                Price = 600.00m,
                Category = "Energetic",
                CreatedAt = seedDate,
                IsActive = true
            },
            new
            {
                Id = therapy3Id,
                Name = "Guided Meditation",
                Description = "Meditation session to reduce anxiety and improve "
                            + "mental well-being.",
                DurationMinutes = 30,
                Price = 300.00m,
                Category = "Mental",
                CreatedAt = seedDate,
                IsActive = true
            }
        );
    }
}