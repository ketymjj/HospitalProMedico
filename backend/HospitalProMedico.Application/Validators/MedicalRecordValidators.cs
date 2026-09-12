using FluentValidation;
using HospitalProMedico.Application.DTOs;

namespace HospitalProMedico.Application.Validators;

public class CreateMedicalRecordValidator : AbstractValidator<CreateMedicalRecordDto>
{
    public CreateMedicalRecordValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorId).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.ChiefComplaint).NotEmpty().MaximumLength(500);
        RuleFor(x => x.History).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Diagnosis).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Prescription).NotEmpty().MaximumLength(2000);
    }
}

public class UpdateMedicalRecordValidator : AbstractValidator<UpdateMedicalRecordDto>
{
    public UpdateMedicalRecordValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorId).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.ChiefComplaint).NotEmpty().MaximumLength(500);
        RuleFor(x => x.History).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Diagnosis).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Prescription).NotEmpty().MaximumLength(2000);
    }
}
