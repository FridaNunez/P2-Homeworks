using ZenCare.Application.DTOs;
using ZenCare.Application.Interfaces;
using ZenCare.Application.Services.Interfaces;
using ZenCare.Domain.Entities;

namespace ZenCare.Application.Services;

public class SpecialistService : ISpecialistService
{
    private readonly IUnitOfWork _uow;

    public SpecialistService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<SpecialistResponseDto>> GetAllAsync()
    {
        var list = await _uow.Specialists.GetActiveAsync();
        return list.Select(MapToResponse);
    }

    public async Task<SpecialistResponseDto?> GetByIdAsync(Guid id)
    {
        var specialist = await _uow.Specialists.GetByIdAsync(id);
        return specialist == null ? null : MapToResponse(specialist);
    }

    public async Task<SpecialistResponseDto> CreateAsync(SpecialistCreateDto dto)
    {
        if (await _uow.Specialists.LicenseExistsAsync(dto.LicenseNumber))
            throw new InvalidOperationException(
                $"A specialist with license {dto.LicenseNumber} already exists.");

        var specialist = new Specialist(dto.FirstName, dto.LastName, dto.Email,
                                        dto.LicenseNumber, dto.Bio)
        {
            Phone = dto.Phone,
            SpecialtiesStr = dto.SpecialtiesStr
        };

        await _uow.Specialists.AddAsync(specialist);
        await _uow.SaveChangesAsync();
        return MapToResponse(specialist);
    }

    public async Task<SpecialistResponseDto> UpdateAsync(SpecialistUpdateDto dto)
    {
        var specialist = await _uow.Specialists.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Specialist {dto.Id} not found.");

        if (await _uow.Specialists.LicenseExistsAsync(dto.LicenseNumber, dto.Id))
            throw new InvalidOperationException("License number is already registered.");

        specialist.FirstName = dto.FirstName;
        specialist.LastName = dto.LastName;
        specialist.Email = dto.Email;
        specialist.Phone = dto.Phone;
        specialist.LicenseNumber = dto.LicenseNumber;
        specialist.Bio = dto.Bio;
        specialist.SpecialtiesStr = dto.SpecialtiesStr;

        _uow.Specialists.Update(specialist);
        await _uow.SaveChangesAsync();
        return MapToResponse(specialist);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var specialist = await _uow.Specialists.GetByIdAsync(id);
        if (specialist == null) return false;
        specialist.IsActive = false;
        _uow.Specialists.Update(specialist);
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetScheduleAsync(
        Guid specialistId, DateTime start, DateTime end)
    {
        var appointments = await _uow.Appointments
            .GetSpecialistScheduleAsync(specialistId, start, end);

        return appointments.Select(a => new AppointmentResponseDto
        {
            Id = a.Id,
            ScheduledAt = a.ScheduledAt,
            Status = a.Status,
            StatusName = a.Status.ToString(),
            PatientId = a.PatientId,
            PatientName = a.Patient?.FullName ?? string.Empty,
            TherapyId = a.TherapyId,
            TherapyName = a.Therapy?.Name ?? string.Empty,
            TherapyDuration = a.Therapy?.DurationMinutes ?? 0,
            TherapyPrice = a.Therapy?.Price ?? 0,
            SpecialistId = specialistId,
            SpecialistName = a.Specialist?.FullName ?? string.Empty
        });
    }

    private static SpecialistResponseDto MapToResponse(Specialist s) => new()
    {
        Id = s.Id,
        FirstName = s.FirstName,
        LastName = s.LastName,
        FullName = s.FullName,
        Email = s.Email,
        Phone = s.Phone,
        LicenseNumber = s.LicenseNumber,
        Bio = s.Bio,
        SpecialtiesStr = s.SpecialtiesStr,
        IsActive = s.IsActive,
        CreatedAt = s.CreatedAt
    };
}