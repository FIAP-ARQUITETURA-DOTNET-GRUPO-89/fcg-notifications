using MediatR;
using FcgNotifications.Application.Responses.Orders;
using FcgNotifications.SharedKernel.Responses;
using FcgNotifications.SharedKernel.Validators;
using OperationResult;

namespace FcgNotifications.Application.Queries.Orders;

public record GetOrdersQuery(int Page = 1, int PageSize = 10)
    : IRequest<Result<PagedResponse<GetOrdersResponse>>>, IValidatableRequest
{
    public string? UserId { get; set; }
    public bool IsAdmin { get; set; }
}
