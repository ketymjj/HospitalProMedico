using FluentValidation;
using HospitalProMedico.Application.DTOs;

namespace HospitalProMedico.Application.Validators;

public class CreateHospitalizationValidator : AbstractValidator<CreateHospitalizationDto>
{
    public CreateHospitalizationValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorId).GreaterThan(0);
        RuleFor(x => x.AdmissionDate).NotEmpty();
        RuleFor(x => x.Room).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Bed).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Diagnosis).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}

public class UpdateHospitalizationValidator : AbstractValidator<UpdateHospitalizationDto>
{
    public UpdateHospitalizationValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorId).GreaterThan(0);
        RuleFor(x => x.AdmissionDate).NotEmpty();
        RuleFor(x => x.Room).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Bed).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Diagnosis).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Status).IsInEnum();
    }
}
