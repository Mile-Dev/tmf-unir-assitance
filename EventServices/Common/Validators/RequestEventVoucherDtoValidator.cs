using EventServices.Domain.Dto.Query;
using FluentValidation;

namespace EventServices.Common.Validators
{
    public class RequestEventVoucherDtoValidator : AbstractValidator<RequestEventVoucherDto>
    {
        public RequestEventVoucherDtoValidator()
        {
            RuleFor(x => x.NameVoucher)
            .MinimumLength(1).WithMessage("Debe existir información en el nombre de voucher.");
        }
    }
}
