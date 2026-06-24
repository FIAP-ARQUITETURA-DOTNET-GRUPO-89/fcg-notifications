using FluentValidation;
using FcgNotifications.Application.Commands.Orders;
using FcgNotifications.Domain.Enums;

namespace FcgNotifications.Application.Validators.Orders;

public class ForceOrderStatusValidator : AbstractValidator<ForceOrderStatusCommand>
{
    public ForceOrderStatusValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.NewStatus)
            .NotEmpty()
            .Must(x => Enum.TryParse<OrderStatus>(x, true, out _))
            .WithMessage($"Status inválido. Valores permitidos: {string.Join(", ", Enum.GetNames<OrderStatus>())}.");
    }
}
