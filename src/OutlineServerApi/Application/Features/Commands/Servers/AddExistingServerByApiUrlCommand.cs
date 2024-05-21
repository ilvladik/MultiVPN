using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Servers;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using OutlineServerApi.Helpers;
using OutlineServerApi.Infrastructure.OutlineApi;
using OutlineServerApi.Infrastructure.OutlineApi.Models;
using SharedKernel.Results;
using System.Security.Claims;

namespace OutlineServerApi.Application.Features.Commands.Servers
{
    public sealed record AddExistingServerByApiUrlCommand(string Name, string ApiUrl, Guid CountryId) : ICommand<ServerResponse>;

    internal class AddExistingServerByApiUrlCommandHandler : ICommandHandler<AddExistingServerByApiUrlCommand, ServerResponse>
    {
        private readonly OutlineContext _context;
        
        private readonly IOutlineApi _outlineApi;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<AddExistingServerByApiUrlCommandHandler> _logger;

        public AddExistingServerByApiUrlCommandHandler(
            OutlineContext context, 
            IOutlineApi outlineApi,
            IHttpContextAccessor contextAccessor,
            ErrorDescriber errorDescriber, 
            ILogger<AddExistingServerByApiUrlCommandHandler> logger)
        {
            _context = context;
            _outlineApi = outlineApi;
            _contextAccessor = contextAccessor;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result<ServerResponse>> Handle(AddExistingServerByApiUrlCommand request, CancellationToken cancellationToken = default)
        {
            Guid createdByUser = Guid.Parse(_contextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Country? country = await _context.Countries
                .SingleOrDefaultAsync(c => c.Id == request.CountryId);

            if (country is null)
            {
                Result<ServerResponse> result = Result<ServerResponse>.FailedNotFound(_errorDescriber.CountryNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            if (_context.Servers.Any(s => s.Hostname == OutlineApiHelper.GetHostname(request.ApiUrl))) 
            {
                Result<ServerResponse> result = Result<ServerResponse>
                    .FailedConflict(_errorDescriber.ServerWithThisAddressAlreadyExists(OutlineApiHelper.GetHostname(request.ApiUrl)));
                _logger.LogWarning(result.ToString());
                return result;
            }
            
            OutlineServer serverInfo;
            try
            {
                serverInfo = await _outlineApi.GetServerAsync(OutlineApiHelper.GetUri(request.ApiUrl));
            }
            catch (Exception)
            {
                Result<ServerResponse> result = Result<ServerResponse>.FailedValidation(_errorDescriber.InvalidServerAddress(request.ApiUrl));
                _logger.LogWarning(result.ToString());
                return result;
            }


            Server server = new Server
            {
                CountryId = country.Id,
                Name = serverInfo.Name,
                Hostname = serverInfo.HostnameForAccessKeys,
                Port = OutlineApiHelper.GetPort(request.ApiUrl),
                ApiPrefix = OutlineApiHelper.GetApiPrefix(request.ApiUrl)
            };

            Key[] keys = (await _outlineApi.GetAllKeysAsync(OutlineApiHelper.GetUri(request.ApiUrl)))
                .Select(outlineKey => new Key
                {
                    ServerId = server.Id,
                    CreatedByUser = createdByUser,
                    InternalId = outlineKey.Id,
                    Name = outlineKey.Name,
                    Password = outlineKey.Password,
                    Port = outlineKey.Port,
                    Method = outlineKey.Method
                })
                .ToArray();
            await _context.Servers.AddAsync(server);
            await _context.Keys.AddRangeAsync(keys);
            await _context.SaveChangesAsync();

            return Result<ServerResponse>.Success(
                new ServerResponse()
                {
                    Id = server.Id,
                    Name = server.Name,
                    ApiUrl = request.ApiUrl,
                    ServerAddress = server.Hostname,
                    IsAvailable = server.IsAvailable,
                    KeysCount = keys.Length,
                    Country = country.Name
                });
        }
    }
}
