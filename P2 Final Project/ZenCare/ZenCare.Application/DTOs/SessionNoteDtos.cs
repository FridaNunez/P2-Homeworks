namespace ZenCare.Application.DTOs;

public class SessionNoteCreateDto
{
    public Guid AppointmentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string RecordedBy { get; set; } = string.Empty;
    public string? Recommendations { get; set; }
}

public class SessionNoteResponseDto
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string RecordedBy { get; set; } = string.Empty;
    public string? Recommendations { get; set; }
    public DateTime CreatedAt { get; set; }
}