using ZenCare.Domain.Entities;

namespace ZenCare.Application.DTOs;

public class AppointmentCreateDto
{
    public Guid PatientId { get; set; }
    public Guid SpecialistId { get; set; }
    public Guid TherapyId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string? ConsultationReason { get; set; }
}

public class AppointmentUpdateDto
{
    public Guid Id { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string? ConsultationReason { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? SessionSummary { get; set; }
}

public class AppointmentResponseDto
{
    public Guid Id { get; set; }
    public DateTime ScheduledAt { get; set; }
    public AppointmentStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? ConsultationReason { get; set; }
    public string? SessionSummary { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid SpecialistId { get; set; }
    public string SpecialistName { get; set; } = string.Empty;
    public Guid TherapyId { get; set; }
    public string TherapyName { get; set; } = string.Empty;
    public decimal TherapyPrice { get; set; }
    public int TherapyDuration { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SessionNoteResponseDto> Notes { get; set; } = new();
}

public class ChangeAppointmentStatusDto
{
    public Guid AppointmentId { get; set; }
    public AppointmentStatus NewStatus { get; set; }
    public string? SessionSummary { get; set; }
}