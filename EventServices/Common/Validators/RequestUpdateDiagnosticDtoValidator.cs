using EventServices.Domain.Dto;
using EventServices.Domain.Dto.Create;
using FluentValidation;

namespace EventServices.Common.Validators
{
    public class RequestUpdateDiagnosticDtoValidator : AbstractValidator<RequestUpdateDiagnosticDto>
    {
        public RequestUpdateDiagnosticDtoValidator()
        {
            RuleFor(x => x.Diagnostic).Length(3, 100).WithMessage("Debe existir información en el nombre de voucher.");                      
        }
    }
}
