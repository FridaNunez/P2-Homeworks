using Microsoft.AspNetCore.Mvc;
using Pet_Rescue.Data;
using Pet_Rescue.Models.DTOs;
using Pet_Rescue.Models.Entities;

namespace Pet_Rescue.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class Pet_Controller : ControllerBase
    {
        private readonly Pet_RescueDbContext _Pet;

        public Pet_Controller(Pet_RescueDbContext context)
        {
            _Pet = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var pet = _Pet.Pet
                     .Select(x => new Pet_Read_Dto
                     {
                         Id = x.Id,
                         Species = x.Species,
                         Breed = x.Breed,
                         Gender = x.Gender,
                         PetName = x.PetName
                     })
                 .ToList();
            return Ok(pet);
        }
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            var Pet = _Pet.Pet.Find(Id);

            if (Pet == null)
                return NotFound();

            var dto = new Pet_Read_Dto
            {
                Id = Id,
                Species = Pet.Species,
                Breed = Pet.Breed,
                Gender = Pet.Gender,
                PetName = Pet.PetName
            };
            return Ok(dto);
        }
        [HttpPost]
        public IActionResult Create(Pet_Create_Dto dto)
        {
            var Pet = new Pet
            {

                Species = dto.Species,
                Breed = dto.Breed,
                Gender = dto.Gender,
                PetName = dto.PetName,
            };
            _Pet.Pet.Add(Pet);
            _Pet.SaveChanges();
            return CreatedAtAction(nameof(Get), new { Id = Pet.Id }, Pet);

        }
        [HttpPut("{Id}")]
                public IActionResult Put(int Id, Pet_Update_Dto Pet_Update_Dto)
                {
                    var Pet = _Pet.Pet.Find(Id); 
                    if (Pet == null)
                         
                    {
                        return NotFound();
                    }
                    Pet.Species = Pet_Update_Dto.Species;
                    Pet.Breed = Pet_Update_Dto.Breed;
                    Pet.Gender = Pet_Update_Dto.Gender;
                    Pet.PetName = Pet_Update_Dto.PetName;
                    _Pet.SaveChanges();
                    return NoContent();
                }
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var Pet = _Pet.Pet.Find(Id);
            if (Pet == null)
                return NotFound();

            _Pet.Pet.Remove(Pet);
            _Pet.SaveChanges();

            return NoContent();
        }
        
    }
}
