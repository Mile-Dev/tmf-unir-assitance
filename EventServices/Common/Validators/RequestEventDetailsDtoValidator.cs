using EventServices.Domain.Dto.Query;
using FluentValidation;

namespace EventServices.Common.Validators
{
    public class RequestEventDetailsDtoValidator : AbstractValidator<RequestEventDetailsDto>
    {
        public RequestEventDetailsDtoValidator()
        {
            RuleFor(x => x.TypeAssistanceIdEvent)
            .GreaterThan(0).WithMessage("Debe seleccionar un tipo de asistencia.");
        }
    }
}
