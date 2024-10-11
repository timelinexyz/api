using Domain.Common;
using MediatR;

namespace Application.Interfaces.Common;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse> { }
