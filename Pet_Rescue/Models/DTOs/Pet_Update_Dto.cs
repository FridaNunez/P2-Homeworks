using System.ComponentModel.DataAnnotations;

namespace Pet_Rescue.Models.DTOs
{
    public class Pet_Update_Dto
    {
        [Required]
        public string Species { get; set; } = string.Empty;

        [Required]
        public string Breed { get; set; } = string.Empty;

        [Required]
        public char Gender { get; set; }

        [Required]
        public string PetName { get; set; } = string.Empty;
    }
}
