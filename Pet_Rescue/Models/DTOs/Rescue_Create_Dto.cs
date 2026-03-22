using System.ComponentModel.DataAnnotations;

namespace Pet_Rescue.Models.DTOs
{
    public class Rescue_Create_Dto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Place { get; set; } = string.Empty;

        [Required]
        public string State { get; set; }
    }
}
