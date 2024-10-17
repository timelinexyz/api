using Core;
using MediatR;

namespace Application.Interfaces.Common;

public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
