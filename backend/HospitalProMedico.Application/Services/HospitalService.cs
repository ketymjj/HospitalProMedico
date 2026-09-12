using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Application.Mappings;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Interfaces;

namespace HospitalProMedico.Application.Services;

public class HospitalService : IHospitalService
{
    private readonly IRepository<Hospital> _hospitals;

    public HospitalService(IRepository<Hospital> hospitals)
    {
        _hospitals = hospitals;
    }

    public async Task<HospitalDto> GetAsync()
    {
        var hospital = (await _hospitals.GetAllAsync()).FirstOrDefault();

        if (hospital is null)
        {
            hospital = new Hospital();
            await _hospitals.AddAsync(hospital);
            await _hospitals.SaveChangesAsync();
        }

        return hospital.ToDto();
    }

    public async Task<HospitalDto> UpdateAsync(UpdateHospitalDto dto)
    {
        var hospital = (await _hospitals.GetAllAsync()).FirstOrDefault();

        if (hospital is null)
        {
            hospital = new Hospital();
            await _hospitals.AddAsync(hospital);
        }

        hospital.Name = dto.Name;
        hospital.Cnpj = dto.Cnpj;
        hospital.Phone = dto.Phone;
        hospital.Email = dto.Email;
        hospital.Address = dto.Address;
        hospital.LogoUrl = dto.LogoUrl;
        hospital.UpdatedAt = DateTime.UtcNow;

        _hospitals.Update(hospital);
        await _hospitals.SaveChangesAsync();

        return hospital.ToDto();
    }
}
