using MediatR;
using SharedKernel.Results;

namespace IdentityServerApi.Application.Abstractions
{
    internal interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

    internal interface IQueryHandler<TQuery, TResponse>
        : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    { }
}
