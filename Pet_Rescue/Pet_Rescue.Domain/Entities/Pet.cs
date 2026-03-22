using System.ComponentModel.DataAnnotations;

namespace Pet_Rescue.Models.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        [Required]
        public string Species { get; set; }

        public string Breed { get; set; }

        public char Gender { get; set; }

        public string PetName { get; set; }
    }
}
