using Microsoft.AspNetCore.Mvc;
using ZenCare.Application.DTOs;
using ZenCare.Application.Services.Interfaces;

namespace ZenCare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionNotesController : ControllerBase
{
    private readonly ISessionNoteService _service;
    public SessionNotesController(ISessionNoteService service) => _service = service;

    [HttpGet("appointment/{appointmentId:guid}")]
    public async Task<IActionResult> GetByAppointment(Guid appointmentId) =>
        Ok(await _service.GetByAppointmentAsync(appointmentId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SessionNoteCreateDto dto)
    {
        try
        {
            return Ok(await _service.CreateAsync(dto));
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
}