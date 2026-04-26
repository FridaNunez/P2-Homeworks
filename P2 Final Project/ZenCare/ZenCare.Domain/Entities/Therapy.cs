namespace ZenCare.Domain.Entities;

public class Therapy : BaseEntity
{
    public Therapy()
    {
        Appointments = new List<Appointment>();
    }

    public Therapy(string name, string description,
                   int durationMinutes, decimal price) : base()
    {
        Name = name;
        Description = description;
        DurationMinutes = durationMinutes;
        Price = price;
        Appointments = new List<Appointment>();
    }

    public Therapy(string name, string description,
                   int durationMinutes, decimal price, string category)
        : this(name, description, durationMinutes, price)
    {
        Category = category;
    }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;

    public ICollection<Appointment> Appointments { get; set; }

    public override string GetDescription() =>
        $"Therapy: {Name} | Duration: {DurationMinutes} min | Price: ${Price:F2}";

    public decimal CalculatePriceWithDiscount(decimal percentage) =>
        Price - (Price * percentage / 100);

    public decimal CalculatePriceWithDiscount(decimal percentage, bool applyVat)
    {
        var discounted = CalculatePriceWithDiscount(percentage);
        return applyVat ? discounted * 1.16m : discounted;
    }
}