namespace ZenCare.Domain.Entities;

public class Appointment : BaseEntity
{
    public Appointment()
    {
        Notes = new List<SessionNote>();
    }

    public Appointment(Guid patientId, Guid specialistId,
                       Guid therapyId, DateTime scheduledAt) : base()
    {
        PatientId = patientId;
        SpecialistId = specialistId;
        TherapyId = therapyId;
        ScheduledAt = scheduledAt;
        Status = AppointmentStatus.Pending;
        Notes = new List<SessionNote>();
    }

    public Appointment(Guid patientId, Guid specialistId,
                       Guid therapyId, DateTime scheduledAt, string reason)
        : this(patientId, specialistId, therapyId, scheduledAt)
    {
        ConsultationReason = reason;
    }

    public DateTime ScheduledAt { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? ConsultationReason { get; set; }
    public string? SessionSummary { get; set; }

    public Guid PatientId { get; set; }
    public Guid SpecialistId { get; set; }
    public Guid TherapyId { get; set; }

    public Patient? Patient { get; set; }
    public Specialist? Specialist { get; set; }
    public Therapy? Therapy { get; set; }

    public ICollection<SessionNote> Notes { get; set; }

    public override string GetDescription() =>
        $"Appointment: {ScheduledAt:dd/MM/yyyy HH:mm} | Status: {Status}";

    public bool CanBeCancelled() =>
        Status == AppointmentStatus.Pending ||
        Status == AppointmentStatus.Confirmed;

    public void Confirm()
    {
        if (Status != AppointmentStatus.Pending)
            throw new InvalidOperationException(
                "Only pending appointments can be confirmed.");
        Status = AppointmentStatus.Confirmed;
    }

    public void Complete(string sessionSummary)
    {
        if (Status != AppointmentStatus.Confirmed)
            throw new InvalidOperationException(
                "Only confirmed appointments can be completed.");
        Status = AppointmentStatus.Completed;
        SessionSummary = sessionSummary;
    }

    public void Cancel()
    {
        if (!CanBeCancelled())
            throw new InvalidOperationException(
                "This appointment cannot be cancelled.");
        Status = AppointmentStatus.Cancelled;
    }
}

public enum AppointmentStatus
{
    Pending = 0,
    Confirmed = 1,
    Completed = 2,
    Cancelled = 3
}