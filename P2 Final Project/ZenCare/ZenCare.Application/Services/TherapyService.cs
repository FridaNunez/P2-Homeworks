using ZenCare.Application.DTOs;
using ZenCare.Application.Interfaces;
using ZenCare.Application.Services.Interfaces;
using ZenCare.Domain.Entities;

namespace ZenCare.Application.Services;

public class TherapyService : ITherapyService
{
    private readonly IUnitOfWork _uow;

    public TherapyService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<TherapyResponseDto>> GetAllAsync()
    {
        var list = await _uow.Therapies.GetActiveAsync();
        return list.Select(MapToResponse);
    }

    public async Task<TherapyResponseDto?> GetByIdAsync(Guid id)
    {
        var therapy = await _uow.Therapies.GetByIdAsync(id);
        return therapy == null ? null : MapToResponse(therapy);
    }

    public async Task<TherapyResponseDto> CreateAsync(TherapyCreateDto dto)
    {
        if (dto.DurationMinutes <= 0)
            throw new ArgumentException("Duration must be greater than 0 minutes.");
        if (dto.Price < 0)
            throw new ArgumentException("Price cannot be negative.");

        var therapy = new Therapy(dto.Name, dto.Description,
                                  dto.DurationMinutes, dto.Price, dto.Category);

        await _uow.Therapies.AddAsync(therapy);
        await _uow.SaveChangesAsync();
        return MapToResponse(therapy);
    }

    public async Task<TherapyResponseDto> UpdateAsync(TherapyUpdateDto dto)
    {
        var therapy = await _uow.Therapies.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Therapy {dto.Id} not found.");

        therapy.Name = dto.Name;
        therapy.Description = dto.Description;
        therapy.DurationMinutes = dto.DurationMinutes;
        therapy.Price = dto.Price;
        therapy.Category = dto.Category;

        _uow.Therapies.Update(therapy);
        await _uow.SaveChangesAsync();
        return MapToResponse(therapy);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var therapy = await _uow.Therapies.GetByIdAsync(id);
        if (therapy == null) return false;
        therapy.IsActive = false;
        _uow.Therapies.Update(therapy);
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TherapyResponseDto>> GetByCategoryAsync(string category)
    {
        var list = await _uow.Therapies.GetByCategoryAsync(category);
        return list.Select(MapToResponse);
    }

    private static TherapyResponseDto MapToResponse(Therapy t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        Description = t.Description,
        DurationMinutes = t.DurationMinutes,
        Price = t.Price,
        Category = t.Category,
        IsActive = t.IsActive,
        CreatedAt = t.CreatedAt
    };
}