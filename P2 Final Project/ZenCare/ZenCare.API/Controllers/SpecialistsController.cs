using Microsoft.AspNetCore.Mvc;
using ZenCare.Application.DTOs;
using ZenCare.Application.Services.Interfaces;

namespace ZenCare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialistsController : ControllerBase
{
    private readonly ISpecialistService _service;
    public SpecialistsController(ISpecialistService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SpecialistCreateDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SpecialistUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");
        try
        {
            return Ok(await _service.UpdateAsync(dto));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}/schedule")]
    public async Task<IActionResult> GetSchedule(
        Guid id, [FromQuery] DateTime start, [FromQuery] DateTime end) =>
        Ok(await _service.GetScheduleAsync(id, start, end));
}