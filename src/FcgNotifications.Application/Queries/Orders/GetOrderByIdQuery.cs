using MediatR;
using FcgNotifications.Application.Responses.Orders;
using FcgNotifications.SharedKernel.Validators;
using OperationResult;

namespace FcgNotifications.Application.Queries.Orders;

public record GetOrderByIdQuery(Guid Id) : IRequest<Result<GetOrderByIdResponse>>, IValidatableRequest
{
    public string? UserId { get; set; }
    public bool IsAdmin { get; set; }
}
