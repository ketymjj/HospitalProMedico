using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalProMedico.Application.Services;

public class HealthPlanService : IHealthPlanService
{
    private readonly IRepository<HealthPlan> _healthPlans;

    public HealthPlanService(IRepository<HealthPlan> healthPlans)
    {
        _healthPlans = healthPlans;
    }

    public async Task<PagedResult<HealthPlanDto>> GetAllAsync(PaginationParams pagination)
    {
        var query = _healthPlans.Query().Include(h => h.Patients).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Search))
        {
            var term = pagination.Search.ToLower();
            query = query.Where(h => h.Name.ToLower().Contains(term) || h.Code.ToLower().Contains(term));
        }

        query = query.OrderBy(h => h.Name);

        var result = await query.ToPagedResultAsync(pagination);

        return new PagedResult<HealthPlanDto>
        {
            Items = result.Items.Select(h => h.ToDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<HealthPlanDto> GetByIdAsync(int id)
    {
        var healthPlan = await _healthPlans.Query().Include(h => h.Patients).FirstOrDefaultAsync(h => h.Id == id)
            ?? throw new NotFoundException("Plano de saúde", id);
        return healthPlan.ToDto();
    }

    public async Task<HealthPlanDto> CreateAsync(CreateHealthPlanDto dto)
    {
        if (await _healthPlans.ExistsAsync(h => h.Code == dto.Code))
        {
            throw new BusinessRuleException("Já existe um plano de saúde com este código.");
        }

        var healthPlan = new HealthPlan
        {
            Name = dto.Name,
            Code = dto.Code,
            Cnpj = dto.Cnpj,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            Type = dto.Type
        };

        await _healthPlans.AddAsync(healthPlan);
        await _healthPlans.SaveChangesAsync();

        return healthPlan.ToDto();
    }

    public async Task<HealthPlanDto> UpdateAsync(int id, UpdateHealthPlanDto dto)
    {
        var healthPlan = await _healthPlans.GetByIdAsync(id) ?? throw new NotFoundException("Plano de saúde", id);

        if (await _healthPlans.ExistsAsync(h => h.Id != id && h.Code == dto.Code))
        {
            throw new BusinessRuleException("Já existe um plano de saúde com este código.");
        }

        healthPlan.Name = dto.Name;
        healthPlan.Code = dto.Code;
        healthPlan.Cnpj = dto.Cnpj;
        healthPlan.Phone = dto.Phone;
        healthPlan.Email = dto.Email;
        healthPlan.Address = dto.Address;
        healthPlan.Type = dto.Type;
        healthPlan.Status = dto.Status;
        healthPlan.UpdatedAt = DateTime.UtcNow;

        _healthPlans.Update(healthPlan);
        await _healthPlans.SaveChangesAsync();

        return healthPlan.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var healthPlan = await _healthPlans.GetByIdAsync(id) ?? throw new NotFoundException("Plano de saúde", id);
        _healthPlans.Remove(healthPlan);
        await _healthPlans.SaveChangesAsync();
    }
}
