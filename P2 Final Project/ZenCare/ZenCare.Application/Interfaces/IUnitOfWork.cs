namespace ZenCare.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPatientRepository Patients { get; }
    ISpecialistRepository Specialists { get; }
    ITherapyRepository Therapies { get; }
    IAppointmentRepository Appointments { get; }
    ISessionNoteRepository SessionNotes { get; }
    Task<int> SaveChangesAsync();
}