using ZenCare.Application.DTOs;
using ZenCare.Application.Interfaces;
using ZenCare.Application.Services.Interfaces;
using ZenCare.Domain.Entities;

namespace ZenCare.Application.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _uow;

    public PatientService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<PatientResponseDto>> GetAllAsync()
    {
        var list = await _uow.Patients.GetActiveAsync();
        return list.Select(MapToResponse);
    }

    public async Task<PatientResponseDto?> GetByIdAsync(Guid id)
    {
        var patient = await _uow.Patients.GetWithAppointmentsAsync(id);
        return patient == null ? null : MapToResponseWithAppointments(patient);
    }

    public async Task<PatientResponseDto> CreateAsync(PatientCreateDto dto)
    {
        if (await _uow.Patients.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException(
                $"A patient with email '{dto.Email}' already exists.");

        var patient = new Patient(dto.FirstName, dto.LastName, dto.Email)
        {
            Phone = dto.Phone,
            DateOfBirth = dto.DateOfBirth,
            MedicalNotes = dto.MedicalNotes
        };

        await _uow.Patients.AddAsync(patient);
        await _uow.SaveChangesAsync();
        return MapToResponse(patient);
    }

    public async Task<PatientResponseDto> UpdateAsync(PatientUpdateDto dto)
    {
        var patient = await _uow.Patients.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Patient {dto.Id} not found.");

        if (await _uow.Patients.EmailExistsAsync(dto.Email, dto.Id))
            throw new InvalidOperationException($"Email '{dto.Email}' is already in use.");

        patient.FirstName = dto.FirstName;
        patient.LastName = dto.LastName;
        patient.Email = dto.Email;
        patient.Phone = dto.Phone;
        patient.DateOfBirth = dto.DateOfBirth;
        patient.MedicalNotes = dto.MedicalNotes;

        _uow.Patients.Update(patient);
        await _uow.SaveChangesAsync();
        return MapToResponse(patient);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var patient = await _uow.Patients.GetByIdAsync(id);
        if (patient == null) return false;

        patient.IsActive = false;          // soft delete
        _uow.Patients.Update(patient);
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetAppointmentHistoryAsync(
        Guid patientId)
    {
        var list = await _uow.Appointments.GetByPatientAsync(patientId);
        return list.Select(MapAppointment);
    }

    // ── Private mappers ───────────────────────────────────────────────────────
    private static PatientResponseDto MapToResponse(Patient p) => new()
    {
        Id = p.Id,
        FirstName = p.FirstName,
        LastName = p.LastName,
        FullName = p.FullName,
        Email = p.Email,
        Phone = p.Phone,
        DateOfBirth = p.DateOfBirth,
        Age = p.CalculateAge(),
        MedicalNotes = p.MedicalNotes,
        IsActive = p.IsActive,
        CreatedAt = p.CreatedAt
    };

    private static PatientResponseDto MapToResponseWithAppointments(Patient p)
    {
        var dto = MapToResponse(p);
        dto.TotalAppointments = p.Appointments?.Count ?? 0;
        return dto;
    }

    private static AppointmentResponseDto MapAppointment(Appointment a) => new()
    {
        Id = a.Id,
        ScheduledAt = a.ScheduledAt,
        Status = a.Status,
        StatusName = a.Status.ToString(),
        ConsultationReason = a.ConsultationReason,
        SessionSummary = a.SessionSummary,
        PatientId = a.PatientId,
        PatientName = a.Patient?.FullName ?? string.Empty,
        SpecialistId = a.SpecialistId,
        SpecialistName = a.Specialist?.FullName ?? string.Empty,
        TherapyId = a.TherapyId,
        TherapyName = a.Therapy?.Name ?? string.Empty,
        TherapyPrice = a.Therapy?.Price ?? 0,
        TherapyDuration = a.Therapy?.DurationMinutes ?? 0,
        CreatedAt = a.CreatedAt
    };
}