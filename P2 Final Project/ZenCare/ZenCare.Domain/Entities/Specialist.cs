namespace ZenCare.Domain.Entities;

public class Specialist : BaseEntity
{
    public Specialist()
    {
        Appointments = new List<Appointment>();
        TherapySpecialties = new List<string>();
    }

    public Specialist(string firstName, string lastName, string email) : base()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Appointments = new List<Appointment>();
        TherapySpecialties = new List<string>();
    }

    public Specialist(string firstName, string lastName, string email,
                      string licenseNumber, string bio)
        : this(firstName, lastName, email)
    {
        LicenseNumber = licenseNumber;
        Bio = bio;
    }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string SpecialtiesStr { get; set; } = string.Empty;

    public List<string> TherapySpecialties { get; set; }
    public ICollection<Appointment> Appointments { get; set; }

    public override string GetDescription() =>
        $"Specialist: {FirstName} {LastName} | License: {LicenseNumber}";

    public string FullName => $"{FirstName} {LastName}";
}