using System.ComponentModel.DataAnnotations;

namespace Pet_Rescue.Models.DTOs
{
    public class Pet_Read_Dto
    {
        public int Id { get; set; }

        public string Species { get; set; } = string.Empty;

        public string Breed { get; set; } = string.Empty;

        public char Gender { get; set; }

        public string PetName { get; set; } = string.Empty;
    }
}
