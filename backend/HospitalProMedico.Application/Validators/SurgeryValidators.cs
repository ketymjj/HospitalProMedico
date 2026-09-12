using FluentValidation;
using HospitalProMedico.Application.DTOs;

namespace HospitalProMedico.Application.Validators;

public class CreateSurgeryValidator : AbstractValidator<CreateSurgeryDto>
{
    public CreateSurgeryValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorId).GreaterThan(0);
        RuleFor(x => x.Type).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Time).NotEmpty().Matches(@"^([01]\d|2[0-3]):([0-5]\d)$");
        RuleFor(x => x.Room).NotEmpty().MaximumLength(20);
        RuleFor(x => x.EstimatedDurationMinutes).GreaterThan(0);
    }
}

public class UpdateSurgeryValidator : AbstractValidator<UpdateSurgeryDto>
{
    public UpdateSurgeryValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorId).GreaterThan(0);
        RuleFor(x => x.Type).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Time).NotEmpty().Matches(@"^([01]\d|2[0-3]):([0-5]\d)$");
        RuleFor(x => x.Room).NotEmpty().MaximumLength(20);
        RuleFor(x => x.EstimatedDurationMinutes).GreaterThan(0);
        RuleFor(x => x.Status).IsInEnum();
    }
}
