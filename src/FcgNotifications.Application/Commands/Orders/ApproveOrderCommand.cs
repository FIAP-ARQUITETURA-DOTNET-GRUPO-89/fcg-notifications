using MediatR;
using OperationResult;

namespace FcgNotifications.Application.Commands.Orders;

public sealed record ApproveOrderCommand(Guid OrderId): IRequest<Result>;
