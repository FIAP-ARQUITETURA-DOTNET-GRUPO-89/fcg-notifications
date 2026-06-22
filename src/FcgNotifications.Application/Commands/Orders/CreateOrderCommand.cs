using System.Text.Json.Serialization;
using MediatR;
using FcgNotifications.Application.Responses.Orders;
using FcgNotifications.Domain.ValueObjects;
using FcgNotifications.SharedKernel.Validators;
using OperationResult;

namespace FcgNotifications.Application.Commands.Orders;

public record CreateOrderCommand(
    string Customer,
    decimal TotalAmount,
    string Street,
    string City,
    string State,
    string Cep)
: IRequest<Result<CreateOrderResponse>>, IValidatableRequest
{
    [JsonIgnore]
    public Address DeliveryAddress => new(Street, City, State, Cep);
}
