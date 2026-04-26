using Microsoft.AspNetCore.Mvc;
using ZenCare.Application.DTOs;
using ZenCare.Application.Services.Interfaces;

namespace ZenCare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TherapiesController : ControllerBase
{
    private readonly ITherapyService _service;
    public TherapiesController(ITherapyService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category) =>
        Ok(await _service.GetByCategoryAsync(category));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TherapyCreateDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TherapyUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");
        try
        {
            return Ok(await _service.UpdateAsync(dto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}