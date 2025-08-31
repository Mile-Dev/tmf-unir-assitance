using EventServices.Domain.Dto;
using FluentValidation;

namespace EventServices.Common.Validators
{
    public class RequestEventValidator : AbstractValidator<RequestEvent>
    {
        public RequestEventValidator()
        {
            RuleFor(x => x.EventVoucher)
            .SetValidator(new RequestEventVoucherDtoValidator());

            RuleFor(x => x.EventDetails)
            .SetValidator(new RequestEventDetailsDtoValidator());

            //   RuleFor(x => x.FieldsAditionalsMok)
            //     .SetValidator(new RequestMokValidator()!); // Si puede ser null, usa `.When(x => x.FieldsAditionalsMok != null)`
        }
    }
}
