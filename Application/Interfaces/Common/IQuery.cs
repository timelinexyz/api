using Core;
using MediatR;

namespace Application.Interfaces.Common;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
