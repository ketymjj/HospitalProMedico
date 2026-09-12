using FluentValidation;
using HospitalProMedico.Application.DTOs;

namespace HospitalProMedico.Application.Validators;

public class CreateDoctorValidator : AbstractValidator<CreateDoctorDto>
{
    public CreateDoctorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Crm).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Specialty).IsInEnum();
        RuleFor(x => x.Cpf).NotEmpty().Matches(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$").WithMessage("CPF inválido. Utilize 11 dígitos.");
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.BirthDate).NotEmpty().LessThan(DateTime.Now);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(300);
    }
}

public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorDto>
{
    public UpdateDoctorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Crm).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Specialty).IsInEnum();
        RuleFor(x => x.Cpf).NotEmpty();
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.BirthDate).NotEmpty().LessThan(DateTime.Now);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Status).IsInEnum();
    }
}
