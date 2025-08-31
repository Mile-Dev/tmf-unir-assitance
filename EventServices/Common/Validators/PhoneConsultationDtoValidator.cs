using EventServices.Domain.Dto.Create;
using FluentValidation;

namespace EventServices.Common.Validators
{
    public class PhoneConsultationDtoValidator : AbstractValidator<PhoneConsultationDto>
    {
        public PhoneConsultationDtoValidator()
        {
                RuleFor(x => x.EventProviderId)
                    .GreaterThan(0).WithMessage("El Evento Provedor no se encuentra asociado correctamente.");

                RuleFor(x => x.ScheduledAt)
                   .NotNull().WithMessage("El registro debe tener fecha de programación.")
                   .NotEmpty().WithMessage("El registro debe tener fecha de programación no debe estar vacio.")
                   .Custom((scheduledAt, context) =>
                   {
                       if (scheduledAt <= DateTime.Now)
                       {
                           context.AddFailure("La fecha de programación debe ser futura.");
                       }
                   });

        }
    }
}
