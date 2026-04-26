using Microsoft.AspNetCore.Mvc;
using ZenCare.Application.DTOs;
using ZenCare.Application.Services.Interfaces;

namespace ZenCare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _service;
    public AppointmentsController(IAppointmentService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }
    [HttpGet("date")]
    public async Task<IActionResult> GetByDate([FromQuery] DateTime date)
    {
        // Convertir a UTC para PostgreSQL
        var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        return Ok(await _service.GetByDateAsync(utcDate));
    }

    [HttpGet("specialist/{specialistId:guid}")]
    public async Task<IActionResult> GetBySpecialist(Guid specialistId) =>
        Ok(await _service.GetBySpecialistAsync(specialistId));

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatient(Guid patientId) =>
        Ok(await _service.GetByPatientAsync(patientId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentCreateDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentUpdateDto dto)
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

    [HttpPatch("status")]
    public async Task<IActionResult> ChangeStatus(
        [FromBody] ChangeAppointmentStatusDto dto)
    {
        try
        {
            return Ok(await _service.ChangeStatusAsync(dto));
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