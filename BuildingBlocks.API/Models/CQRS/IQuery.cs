using MediatR;

namespace BuildingBlocks.API.Models.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull;