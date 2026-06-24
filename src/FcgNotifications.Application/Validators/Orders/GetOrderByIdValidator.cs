using FluentValidation;
using FcgNotifications.Application.Queries.Orders;
using FcgNotifications.SharedKernel.Validators;

namespace FcgNotifications.Application.Validators.Orders;

public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>, IValidatableRequest
{
    public GetOrderByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
