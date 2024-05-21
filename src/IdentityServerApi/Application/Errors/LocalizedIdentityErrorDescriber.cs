using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace IdentityServerApi.Application.Errors
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {

        private readonly IStringLocalizer<LocalizedIdentityErrorDescriber> _stringLocalizer;

        public LocalizedIdentityErrorDescriber(IStringLocalizer<LocalizedIdentityErrorDescriber> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public override IdentityError DefaultError() 
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = _stringLocalizer[nameof(DefaultError)]
            }; 
        }
        public override IdentityError ConcurrencyFailure() 
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = _stringLocalizer[nameof(ConcurrencyFailure)]
            }; 
        }
        public override IdentityError PasswordMismatch() 
        { 
            return new IdentityError 
            { 
                Code = nameof(PasswordMismatch), 
                Description = _stringLocalizer[nameof(PasswordMismatch)] 
            }; 
        }
        public override IdentityError InvalidToken() 
        { 
            return new IdentityError 
            { 
                Code = nameof(InvalidToken), 
                Description = _stringLocalizer[nameof(InvalidToken)] 
            }; 
        }
        public override IdentityError LoginAlreadyAssociated() 
        { 
            return new IdentityError 
            { 
                Code = nameof(LoginAlreadyAssociated), 
                Description = _stringLocalizer[nameof(LoginAlreadyAssociated)] 
            }; 
        }
        public override IdentityError InvalidUserName(string userName) 
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = _stringLocalizer[nameof(InvalidUserName), userName] 
            }; 
        }
        public override IdentityError InvalidEmail(string email) 
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = _stringLocalizer[nameof(InvalidEmail), email]
            }; 
        }
        public override IdentityError DuplicateUserName(string userName) 
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = _stringLocalizer[nameof(DuplicateUserName), userName] 
            }; 
        }
        public override IdentityError DuplicateEmail(string email) 
        { 
            return new IdentityError 
            { 
                Code = nameof(DuplicateEmail), 
                Description = _stringLocalizer[nameof(DuplicateEmail), email]
            }; 
        }
        public override IdentityError InvalidRoleName(string role) 
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = _stringLocalizer[nameof(InvalidRoleName), role]
            };
        }
        public override IdentityError DuplicateRoleName(string role) 
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = _stringLocalizer[nameof(DuplicateRoleName), role]
            };
        }
        public override IdentityError UserAlreadyHasPassword() 
        { 
            return new IdentityError 
            { 
                Code = nameof(UserAlreadyHasPassword), 
                Description = _stringLocalizer[nameof(UserAlreadyHasPassword)] 
            }; 
        }
        public override IdentityError UserLockoutNotEnabled() 
        { 
            return new IdentityError 
            { 
                Code = nameof(UserLockoutNotEnabled), 
                Description = _stringLocalizer[nameof(UserLockoutNotEnabled)] 
            };
        }
        public override IdentityError UserAlreadyInRole(string role) 
        { 
            return new IdentityError 
            { 
                Code = nameof(UserAlreadyInRole), 
                Description = _stringLocalizer[nameof(UserAlreadyInRole), role]
            };
        }
        public override IdentityError UserNotInRole(string role) 
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = _stringLocalizer[nameof(UserNotInRole), role] 
            }; 
        }
        public override IdentityError PasswordTooShort(int length) 
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = _stringLocalizer[nameof(PasswordTooShort), length] 
            }; 
        }
        public override IdentityError PasswordRequiresNonAlphanumeric() 
        { 
            return new IdentityError 
            { 
                Code = nameof(PasswordRequiresNonAlphanumeric), 
                Description = _stringLocalizer[nameof(PasswordRequiresNonAlphanumeric)] 
            }; 
        }
        public override IdentityError PasswordRequiresDigit() 
        { 
            return new IdentityError 
            { 
                Code = nameof(PasswordRequiresDigit), 
                Description = _stringLocalizer[nameof(PasswordRequiresDigit)] 
            }; 
        }
        public override IdentityError PasswordRequiresLower() 
        { 
            return new IdentityError 
            { 
                Code = nameof(PasswordRequiresLower), 
                Description = _stringLocalizer[nameof(PasswordRequiresLower)] 
            };
        }
        public override IdentityError PasswordRequiresUpper() 
        { 
            return new IdentityError 
            { 
                Code = nameof(PasswordRequiresUpper), 
                Description = _stringLocalizer[nameof(PasswordRequiresUpper)] 
            }; 
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = _stringLocalizer[nameof(PasswordRequiresUniqueChars), uniqueChars]
            };
        }
    }
}
