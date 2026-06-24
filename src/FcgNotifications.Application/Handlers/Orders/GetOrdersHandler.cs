using MediatR;
using FcgNotifications.Application.Mappers;
using FcgNotifications.Application.Queries.Orders;
using FcgNotifications.Application.Responses.Orders;
using FcgNotifications.Domain.Repositories.Orders;
using FcgNotifications.SharedKernel.Responses;
using OperationResult;

namespace FcgNotifications.Application.Handlers.Orders;

public sealed class GetOrdersHandler(
    IOrderRepository orderRepository)
: IRequestHandler<GetOrdersQuery, Result<PagedResponse<GetOrdersResponse>>>
{
    public async Task<Result<PagedResponse<GetOrdersResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var customerId = request.IsAdmin
            ? null
            : request.UserId;

        var (items, totalCount) = await orderRepository.GetPagedAsNoTrackingAsync(
            request.Page,
            request.PageSize,
            customerId,
            cancellationToken);

        var pagedResponse = new PagedResponse<GetOrdersResponse>(items.ToGetOrdersResponseList(), totalCount, request.Page, request.PageSize);

        return Result.Success(pagedResponse);
    }
}
