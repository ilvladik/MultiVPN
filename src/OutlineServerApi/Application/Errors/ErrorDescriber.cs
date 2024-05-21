using Microsoft.Extensions.Localization;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Errors
{
    internal class ErrorDescriber
    {
        private readonly IStringLocalizer<ErrorDescriber> _stringLocalizer;

        public ErrorDescriber(IStringLocalizer<ErrorDescriber> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public Error CountryWithThisNameAlreadyExists(string name)
        {
            return new Error()
            {
                Code = nameof(CountryWithThisNameAlreadyExists),
                Description = _stringLocalizer[nameof(CountryWithThisNameAlreadyExists), name]
            };
        }

        public Error CountryNotFound()
        {
            return new Error()
            {
                Code = nameof(CountryNotFound),
                Description = _stringLocalizer[nameof(CountryNotFound)]
            };
        }

        public Error KeyNotFound()
        {
            return new Error()
            {
                Code = nameof(KeyNotFound),
                Description = _stringLocalizer[nameof(KeyNotFound)]
            };
        }

        public Error OnlyOneKeyAllowed()
        {
            return new Error()
            {
                Code = nameof(OnlyOneKeyAllowed),
                Description = _stringLocalizer[nameof(OnlyOneKeyAllowed)]
            };
        }


        public Error ServerNotFound()
        {
            return new Error()
            {
                Code = nameof(ServerNotFound),
                Description = _stringLocalizer[nameof(ServerNotFound)]
            };
        }

        public Error ServerNotAvailable(string ip)
        {
            return new Error()
            {
                Code = nameof(ServerNotAvailable),
                Description = _stringLocalizer[nameof(ServerNotAvailable), ip]
            };
        }

        public Error AllServersNotAvailable()
        {
            return new Error()
            {
                Code = nameof(AllServersNotAvailable),
                Description = _stringLocalizer[nameof(AllServersNotAvailable)]
            };
        }

        public Error ServersHaveNotSameCountries(string ipOne, string ipTwo)
        {
            return new Error()
            {
                Code = nameof(ServersHaveNotSameCountries),
                Description = _stringLocalizer[nameof(ServersHaveNotSameCountries), ipOne, ipTwo]
            };
        }

        public Error InvalidServerAddress(string serverAddress)
        {
            return new Error()
            {
                Code = nameof(InvalidServerAddress),
                Description = _stringLocalizer[nameof(InvalidServerAddress), serverAddress]
            };
        }

        public Error ServerWithThisAddressAlreadyExists(string serverAddress)
        {
            return new Error()
            {
                Code = nameof(ServerWithThisAddressAlreadyExists),
                Description = _stringLocalizer[nameof(ServerWithThisAddressAlreadyExists), serverAddress]
            };
        }
        public Error KeyNotBelongToYou()
        {
            return new Error()
            {
                Code = nameof(KeyNotBelongToYou),
                Description = _stringLocalizer[nameof(KeyNotBelongToYou)]
            };
        }
    }
}
