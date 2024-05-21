using Microsoft.Extensions.Localization;
using SharedKernel.Results;

namespace IdentityServerApi.Application.Errors
{
    internal class ErrorDescriber
    {
        private readonly IStringLocalizer<ErrorDescriber> _stringLocalizer;
        public ErrorDescriber(IStringLocalizer<ErrorDescriber> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
        public Error InvalidCode()
        {
            return new Error
            {
                Code = nameof(InvalidCode),
                Description = _stringLocalizer[nameof(InvalidCode)]
            };
        }

        public Error EmailNotConfirmed()
        {
            return new Error
            {
                Code = nameof(EmailNotConfirmed),
                Description = _stringLocalizer[nameof(EmailNotConfirmed)]
            };
        }

        public Error InvalidRefreshToken()
        {
            return new Error
            {
                Code = nameof(InvalidRefreshToken),
                Description = _stringLocalizer[nameof(InvalidRefreshToken)]
            };
        }

        public Error SignInIsLockedOut()
        {
            return new Error
            {
                Code = nameof(SignInIsLockedOut),
                Description = _stringLocalizer[nameof(SignInIsLockedOut)]
            };
        }

        public Error EmailNotFound(string email)
        {
            return new Error
            {
                Code = nameof(EmailNotFound),
                Description = _stringLocalizer[nameof(EmailNotFound), email]
            };
        }

        public Error InvalidEmailOrPassword()
        {
            return new Error
            {
                Code = nameof(InvalidEmailOrPassword),
                Description = _stringLocalizer[nameof(InvalidEmailOrPassword)]
            };
        }
    }
}
