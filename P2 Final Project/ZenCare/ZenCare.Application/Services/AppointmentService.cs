using ZenCare.Application.DTOs;
using ZenCare.Application.Interfaces;
using ZenCare.Application.Services.Interfaces;
using ZenCare.Domain.Entities;

namespace ZenCare.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _uow;

    public AppointmentService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
    {
        var all = await _uow.Appointments.GetAllAsync();
        var result = new List<AppointmentResponseDto>();
        foreach (var a in all)
        {
            var detail = await _uow.Appointments.GetWithDetailsAsync(a.Id);
            if (detail != null) result.Add(MapToResponse(detail));
        }
        return result;
    }

    public async Task<AppointmentResponseDto?> GetByIdAsync(Guid id)
    {
        var appointment = await _uow.Appointments.GetWithDetailsAsync(id);
        return appointment == null ? null : MapToResponse(appointment);
    }

    public async Task<AppointmentResponseDto> CreateAsync(AppointmentCreateDto dto)
    {
        if (!await _uow.Patients.ExistsAsync(dto.PatientId))
            throw new KeyNotFoundException("Patient not found.");
        if (!await _uow.Specialists.ExistsAsync(dto.SpecialistId))
            throw new KeyNotFoundException("Specialist not found.");
        if (!await _uow.Therapies.ExistsAsync(dto.TherapyId))
            throw new KeyNotFoundException("Therapy not found.");

        // Convertir a UTC para PostgreSQL
        var scheduledAtUtc = dto.ScheduledAt.Kind == DateTimeKind.Utc
            ? dto.ScheduledAt
            : DateTime.SpecifyKind(dto.ScheduledAt, DateTimeKind.Utc);

        if (await _uow.Appointments.HasScheduleConflictAsync(
                dto.SpecialistId, scheduledAtUtc))
            throw new InvalidOperationException(
                "The specialist already has an appointment at that time.");

        if (scheduledAtUtc < DateTime.UtcNow)
            throw new ArgumentException("Cannot schedule appointments in the past.");

        var appointment = new Appointment(
            dto.PatientId, dto.SpecialistId, dto.TherapyId,
            scheduledAtUtc, dto.ConsultationReason ?? string.Empty);

        await _uow.Appointments.AddAsync(appointment);
        await _uow.SaveChangesAsync();

        var detail = await _uow.Appointments.GetWithDetailsAsync(appointment.Id);
        return MapToResponse(detail!);
    }

    public async Task<AppointmentResponseDto> UpdateAsync(AppointmentUpdateDto dto)
    {
        var appointment = await _uow.Appointments.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Appointment {dto.Id} not found.");

        appointment.ScheduledAt = dto.ScheduledAt;
        appointment.ConsultationReason = dto.ConsultationReason;

        _uow.Appointments.Update(appointment);
        await _uow.SaveChangesAsync();

        var detail = await _uow.Appointments.GetWithDetailsAsync(appointment.Id);
        return MapToResponse(detail!);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var appointment = await _uow.Appointments.GetByIdAsync(id);
        if (appointment == null) return false;
        appointment.IsActive = false;
        _uow.Appointments.Update(appointment);
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<AppointmentResponseDto> ChangeStatusAsync(ChangeAppointmentStatusDto dto)
    {
        var appointment = await _uow.Appointments.GetByIdAsync(dto.AppointmentId)
            ?? throw new KeyNotFoundException($"Appointment {dto.AppointmentId} not found.");

        switch (dto.NewStatus)
        {
            case AppointmentStatus.Confirmed:
                appointment.Confirm();
                break;
            case AppointmentStatus.Completed:
                appointment.Complete(dto.SessionSummary ?? "No summary provided.");
                break;
            case AppointmentStatus.Cancelled:
                appointment.Cancel();
                break;
            default:
                throw new ArgumentException("Invalid status transition.");
        }

        _uow.Appointments.Update(appointment);
        await _uow.SaveChangesAsync();

        var detail = await _uow.Appointments.GetWithDetailsAsync(appointment.Id);
        return MapToResponse(detail!);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByDateAsync(DateTime date)
    {
        var list = await _uow.Appointments.GetByDateAsync(date);
        return list.Select(MapToResponse);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetBySpecialistAsync(Guid specialistId)
    {
        var list = await _uow.Appointments.GetBySpecialistAsync(specialistId);
        return list.Select(MapToResponse);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByPatientAsync(Guid patientId)
    {
        var list = await _uow.Appointments.GetByPatientAsync(patientId);
        return list.Select(MapToResponse);
    }

    private static AppointmentResponseDto MapToResponse(Appointment a) => new()
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
        CreatedAt = a.CreatedAt,
        Notes = a.Notes?.Select(n => new SessionNoteResponseDto
        {
            Id = n.Id,
            AppointmentId = n.AppointmentId,
            Content = n.Content,
            RecordedBy = n.RecordedBy,
            Recommendations = n.Recommendations,
            CreatedAt = n.CreatedAt
        }).ToList() ?? new()
    };
}