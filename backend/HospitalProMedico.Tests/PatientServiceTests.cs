using HospitalProMedico.Application.Common;
using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Services;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Infrastructure.Repositories;
using Xunit;

namespace HospitalProMedico.Tests;

public class PatientServiceTests
{
    private static PatientService CreateService(out HospitalProMedico.Infrastructure.Data.AppDbContext context)
    {
        context = TestDbContextFactory.Create();
        var patients = new Repository<Patient>(context);
        var healthPlans = new Repository<HealthPlan>(context);
        return new PatientService(patients, healthPlans);
    }

    private static CreatePatientDto ValidPatient(string cpf = "12345678901") => new()
    {
        Name = "Maria Teste",
        Cpf = cpf,
        BirthDate = new DateTime(1985, 5, 20),
        Gender = Gender.FEMININO,
        Phone = "(11) 98888-0000",
        Email = "maria.teste@email.com",
        Address = "Rua B, 200",
        City = "São Paulo",
        State = "SP",
        ZipCode = "02000-000"
    };

    [Fact]
    public async Task CreateAsync_ShouldPersistPatient_WithActiveStatus()
    {
        var service = CreateService(out _);

        var result = await service.CreateAsync(ValidPatient());

        Assert.NotEqual(0, result.Id);
        Assert.Equal(RegisterStatus.ATIVO, result.Status);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCpfAlreadyExists()
    {
        var service = CreateService(out _);
        await service.CreateAsync(ValidPatient("11122233344"));

        await Assert.ThrowsAsync<BusinessRuleException>(() => service.CreateAsync(ValidPatient("11122233344")));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenPatientDoesNotExist()
    {
        var service = CreateService(out _);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(999));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemovePatient()
    {
        var service = CreateService(out _);
        var created = await service.CreateAsync(ValidPatient("55566677788"));

        await service.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(created.Id));
    }
}
