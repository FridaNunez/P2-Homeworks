namespace ZenCare.Domain.Entities;

public class Patient : BaseEntity
{
    public Patient()
    {
        Appointments = new List<Appointment>();
    }

    public Patient(string firstName, string lastName, string email) : base()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Appointments = new List<Appointment>();
    }

    public Patient(string firstName, string lastName, string email,
                   string phone, DateTime dateOfBirth)
        : this(firstName, lastName, email)
    {
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? MedicalNotes { get; set; }

    public ICollection<Appointment> Appointments { get; set; }

    public override string GetDescription() =>
        $"Patient: {FirstName} {LastName} | Email: {Email}";

    public int CalculateAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    public string FullName => $"{FirstName} {LastName}";
}