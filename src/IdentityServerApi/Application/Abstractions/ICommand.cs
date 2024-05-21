using MediatR;
using SharedKernel.Results;

namespace IdentityServerApi.Application.Abstractions
{
    internal interface ICommand : IRequest<Result> { }

    internal interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

    internal interface ICommandHandler<TCommand>
        : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    { }

    internal interface ICommandHandler<TCommand, TResponse>
        : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    { }
}
