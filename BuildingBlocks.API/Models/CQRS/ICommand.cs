using MediatR;

namespace BuildingBlocks.API.Models.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : ICommand<Unit>;