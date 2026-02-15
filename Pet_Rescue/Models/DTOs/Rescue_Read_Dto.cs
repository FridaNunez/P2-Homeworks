namespace Pet_Rescue.Models.DTOs
{
    public class Rescue_Read_Dto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Place { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;
    }
}
