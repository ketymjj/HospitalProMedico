using HospitalProMedico.Application.DTOs;
using HospitalProMedico.Application.Validators;
using HospitalProMedico.Domain.Enums;
using Xunit;

namespace HospitalProMedico.Tests;

public class ValidatorTests
{
    [Fact]
    public void CreatePatientValidator_ShouldFail_WhenRequiredFieldsMissing()
    {
        var validator = new CreatePatientValidator();
        var dto = new CreatePatientDto();

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        Assert.Contains(result.Errors, e => e.PropertyName == "Cpf");
    }

    [Fact]
    public void CreatePatientValidator_ShouldPass_WithValidData()
    {
        var validator = new CreatePatientValidator();
        var dto = new CreatePatientDto
        {
            Name = "João da Silva",
            Cpf = "12345678901",
            BirthDate = new DateTime(1990, 1, 1),
            Gender = Gender.MASCULINO,
            Phone = "(11) 99999-0000",
            Email = "joao@email.com",
            Address = "Rua A, 100",
            City = "São Paulo",
            State = "SP",
            ZipCode = "01000-000"
        };

        var result = validator.Validate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateAppointmentValidator_ShouldFail_WhenTimeFormatInvalid()
    {
        var validator = new CreateAppointmentValidator();
        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            DoctorId = 1,
            Date = DateTime.Today,
            Time = "25:99",
            Type = AppointmentType.RETORNO
        };

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Time");
    }

    [Fact]
    public void LoginRequestValidator_ShouldFail_WhenEmailInvalid()
    {
        var validator = new LoginRequestValidator();
        var dto = new LoginRequestDto { Email = "not-an-email", Password = "123456" };

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
    }
}
