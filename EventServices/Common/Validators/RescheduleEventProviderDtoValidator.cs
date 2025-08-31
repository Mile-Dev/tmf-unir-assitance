using EventServices.Domain.Dto.Create;
using FluentValidation;

namespace EventServices.Common.Validators
{
    public class RescheduleEventProviderDtoValidator : AbstractValidator<RescheduleEventProviderDto>
    {
        public RescheduleEventProviderDtoValidator()
        {
            RuleFor(x => x.ScheduledAppointment)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("La fecha de ser mayor al tiempo actual.");                
        }
    }
}
