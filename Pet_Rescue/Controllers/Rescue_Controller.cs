using Microsoft.AspNetCore.Mvc;
using Pet_Rescue.Data;
using Pet_Rescue.Models.DTOs;
using Pet_Rescue.Models.Entities;

namespace Rescue_Rescue.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class Rescue_Controller : ControllerBase
    {
        private readonly Pet_RescueDbContext _Rescue;

        public Rescue_Controller(Pet_RescueDbContext context)
        {
            _Rescue = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var Rescue = _Rescue.Rescue
                     .Select(x => new Rescue_Read_Dto
                     {
                         Id = x.Id,
                         Date = x.Date,
                         Place = x.Place,
                         State = x.State
                     })
                 .ToList();
            return Ok(Rescue);
        }
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            var Rescue = _Rescue.Rescue.Find(Id);

            if (Rescue == null)
                return NotFound();

            var dto = new Rescue_Read_Dto
            {
                Id = Id,
                Date = Rescue.Date,
                Place = Rescue.Place,
                State = Rescue.State
            };
            return Ok(dto);
        }
        [HttpPost]
        public IActionResult Create(Rescue_Create_Dto dto)
        {
            var Rescue = new Rescue
            {

                Date = dto.Date,
                Place = dto.Place,
                State = dto.State
            };
            _Rescue.Rescue.Add(Rescue);
            _Rescue.SaveChanges();
            return CreatedAtAction(nameof(Get), new { Id = Rescue.Id }, Rescue);

        }
        [HttpPut("{Id}")]
        public IActionResult Put(int Id, Rescue_Update_Dto Rescue_Update_Dto)
        {
            var Rescue = _Rescue.Rescue.Find(Id);
            if (Rescue == null)

            {
                return NotFound();
            }
            Rescue.Date = Rescue_Update_Dto.Date;
            Rescue.Place = Rescue_Update_Dto.Place;
            Rescue.State = Rescue_Update_Dto.State;
            _Rescue.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var Rescue = _Rescue.Rescue.Find(Id);
            if (Rescue == null)
                return NotFound();

            _Rescue.Rescue.Remove(Rescue);
            _Rescue.SaveChanges();

            return NoContent();
        }

    }
}

