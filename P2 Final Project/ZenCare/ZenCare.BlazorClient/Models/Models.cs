namespace ZenCare.BlazorClient.Models;

public class PatientModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Age { get; set; }
    public string? MedicalNotes { get; set; }
    public bool IsActive { get; set; }
    public int TotalAppointments { get; set; }
}

public class PatientCreateModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-30);
    public string? MedicalNotes { get; set; }
}

public class SpecialistModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string SpecialtiesStr { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class SpecialistCreateModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string SpecialtiesStr { get; set; } = string.Empty;
}

public class TherapyModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class TherapyCreateModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; } = 60;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
}

public class AppointmentModel
{
    public Guid Id { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int Status { get; set; }
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
    public List<SessionNoteModel> Notes { get; set; } = new();
}

public class AppointmentCreateModel
{
    public Guid PatientId { get; set; }
    public Guid SpecialistId { get; set; }
    public Guid TherapyId { get; set; }
    public DateTime ScheduledAt { get; set; } = DateTime.Now.AddDays(1);
    public string? ConsultationReason { get; set; }
}

public class ChangeStatusModel
{
    public Guid AppointmentId { get; set; }
    public int NewStatus { get; set; }
    public string? SessionSummary { get; set; }
}

public class SessionNoteModel
{
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string RecordedBy { get; set; } = string.Empty;
    public string? Recommendations { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SessionNoteCreateModel
{
    public Guid AppointmentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string RecordedBy { get; set; } = string.Empty;
    public string? Recommendations { get; set; }
}