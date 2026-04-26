using ZenCare.Application.Interfaces;
using ZenCare.Infrastructure.Data;
using ZenCare.Infrastructure.Repositories;

namespace ZenCare.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ZenCareDbContext _context;

    public IPatientRepository Patients { get; }
    public ISpecialistRepository Specialists { get; }
    public ITherapyRepository Therapies { get; }
    public IAppointmentRepository Appointments { get; }
    public ISessionNoteRepository SessionNotes { get; }

    public UnitOfWork(ZenCareDbContext context)
    {
        _context = context;
        Patients = new PatientRepository(context);
        Specialists = new SpecialistRepository(context);
        Therapies = new TherapyRepository(context);
        Appointments = new AppointmentRepository(context);
        SessionNotes = new SessionNoteRepository(context);
    }

    public async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public void Dispose() =>
        _context.Dispose();
}