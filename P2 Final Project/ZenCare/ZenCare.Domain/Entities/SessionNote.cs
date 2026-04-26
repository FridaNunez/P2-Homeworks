namespace ZenCare.Domain.Entities;

public class SessionNote : BaseEntity
{
    public SessionNote() { }

    public SessionNote(Guid appointmentId, string content,
                       string recordedBy) : base()
    {
        AppointmentId = appointmentId;
        Content = content;
        RecordedBy = recordedBy;
    }

    public SessionNote(Guid appointmentId, string content,
                       string recordedBy, string recommendations)
        : this(appointmentId, content, recordedBy)
    {
        Recommendations = recommendations;
    }

    public Guid AppointmentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string RecordedBy { get; set; } = string.Empty;
    public string? Recommendations { get; set; }

    public Appointment? Appointment { get; set; }

    public override string GetDescription() =>
        $"Note by: {RecordedBy} | Date: {CreatedAt:dd/MM/yyyy}";
}