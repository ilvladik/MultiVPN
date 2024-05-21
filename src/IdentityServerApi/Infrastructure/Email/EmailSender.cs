using IdentityServerApi.Core.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using IdentityServerApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using MailKit.Security;

namespace IdentityServerApi.Infrastructure.Email
{
    internal class EmailSender : IEmailSender<User>
    {
        private readonly IStringLocalizer<EmailSender> _stringLocalizer;

        private readonly EmailOptions _options;

        public EmailSender(IStringLocalizer<EmailSender> stringLocalizer, IOptions<EmailOptions> options)
        {
            _stringLocalizer = stringLocalizer;
            _options = options.Value;
        }

        public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            string subject = _stringLocalizer["EmailConfirmationSubject"];
            string message = _stringLocalizer["EmailConfirmationMessage", confirmationLink];
            await Execute(email, subject, message);
        }

        public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            string subject = _stringLocalizer["PasswordResetCodeSubject"];
            string message = _stringLocalizer["PasswordResetCodeMessage", resetCode];
            await Execute(email, subject, message);
        }

        public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            string subject = _stringLocalizer["PasswordResetLinkSubject"];
            string message = _stringLocalizer["PasswordResetLinkMessage", resetLink];
            await Execute(email, subject, message);
        }

        private async Task Execute(string toEmail, string subject, string message)
        {
            using MimeMessage emailMessage = new MimeMessage(
                from: [new MailboxAddress("", _options.From)],
                to: [new MailboxAddress("", toEmail)],
                subject: subject,
                body: new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                });

            using SmtpClient client = new SmtpClient();
            await client.ConnectAsync(_options.Smtp, _options.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_options.Address, _options.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
