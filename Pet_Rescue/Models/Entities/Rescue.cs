using System.ComponentModel.DataAnnotations;

namespace Pet_Rescue.Models.Entities
{
    public class Rescue
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Place { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;
    }
}
