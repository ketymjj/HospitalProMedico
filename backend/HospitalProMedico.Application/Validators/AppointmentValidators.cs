using FluentValidation;
using HospitalProMedico.Application.DTOs;

namespace HospitalProMedico.Application.Validators;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorId).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Time).NotEmpty().Matches(@"^([01]\d|2[0-3]):([0-5]\d)$").WithMessage("Horário inválido. Utilize o formato HH:mm.");
        RuleFor(x => x.Type).IsInEnum();
    }
}

public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentDto>
{
    public UpdateAppointmentValidator()
    {
        RuleFor(x => x.PatientId).GreaterThan(0);
        RuleFor(x => x.DoctorId).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Time).NotEmpty().Matches(@"^([01]\d|2[0-3]):([0-5]\d)$").WithMessage("Horário inválido. Utilize o formato HH:mm.");
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Status).IsInEnum();
    }
}
