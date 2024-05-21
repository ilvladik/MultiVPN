using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Keys;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using OutlineServerApi.Helpers;
using OutlineServerApi.Infrastructure.OutlineApi;
using OutlineServerApi.Infrastructure.OutlineApi.Models;
using SharedKernel.Results;
using System.Security.Claims;

namespace OutlineServerApi.Application.Features.Commands.Keys
{
    public sealed record CreateNewKeyCommand(string Name, Guid CountryId) : ICommand<KeyResponse>;

    internal class CreateNewKeyCommandHandler : ICommandHandler<CreateNewKeyCommand, KeyResponse>
    {
        private readonly OutlineContext _context;

        private readonly IOutlineApi _outlineApi;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IAccessUriProvider _accessUriProvider;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<CreateNewKeyCommandHandler> _logger;

        public CreateNewKeyCommandHandler(
            OutlineContext context, 
            IOutlineApi outlineApi,
            IHttpContextAccessor contextAccessor,
            IAccessUriProvider accessUriProvider,
            ErrorDescriber errorDescriber, 
            ILogger<CreateNewKeyCommandHandler> logger)
        {
            _context = context;
            _outlineApi = outlineApi;
            _contextAccessor = contextAccessor;
            _accessUriProvider = accessUriProvider;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result<KeyResponse>> Handle(CreateNewKeyCommand request, CancellationToken cancellationToken = default)
        {
            Guid createdByUser = Guid.Parse(_contextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string[] roles = _contextAccessor.HttpContext!.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            Country? country = await _context.Countries.SingleOrDefaultAsync(c => c.Id == request.CountryId);
            if (country is null)
            {
                Result<KeyResponse> result = Result<KeyResponse>.FailedNotFound(_errorDescriber.CountryNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            if (_context.Keys.Any(k => k.CreatedByUser == createdByUser) && !roles.Any(r => r == "Admin"))
            {
                Result<KeyResponse> result = Result<KeyResponse>.FailedValidation(_errorDescriber.OnlyOneKeyAllowed());
                _logger.LogWarning(result.ToString());
                return result;
            }

            Server[] servers = await _context.Servers
                .Include(s => s.Keys)
                .Where(s => s.IsAvailable && s.CountryId == request.CountryId)
                .ToArrayAsync();

            Server? server = servers.MinBy(s => s.Keys.Count);
            if (server is null)
            {
                Result<KeyResponse> result = Result<KeyResponse>.FailedNotFound(_errorDescriber.AllServersNotAvailable());
                _logger.LogWarning(result.ToString());
                return result;
            }

            OutlineKey outlineKey = await _outlineApi
                .CreateKeyAsync(OutlineApiHelper.GetUri(server.Hostname, server.Port, server.ApiPrefix));

            Key key = new Key
            {
                ServerId = server.Id,
                CreatedByUser = createdByUser,
                InternalId = outlineKey.Id,
                Name = request.Name,
                Password = outlineKey.Password,
                Port = outlineKey.Port,
                Method = outlineKey.Method
            };
            await _context.AddAsync(key);
            await _context.SaveChangesAsync();

            return Result<KeyResponse>.Success(
                new KeyResponse()
                {
                    Id = key.Id,
                    Name = key.Name,
                    ServerAddress = server.Hostname,
                    CreatedByUser = createdByUser,
                    AccessUri = _accessUriProvider.GetAccessUri(key.Id).AbsoluteUri,
                    Country = country.Name
                });
        }
    }
}
