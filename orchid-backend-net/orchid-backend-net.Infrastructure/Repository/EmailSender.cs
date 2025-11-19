using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Infrastructure.Service.GmailSettings;

namespace orchid_backend_net.Infrastructure.Repository
{
    public class EmailSender(IOptions<GmailOptions> options) : IEmailSender
    {
        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            TokenResponse tokenResponse = new()
            {
                RefreshToken = options.Value.RefreshToken,
            };

            UserCredential creds = new(new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = options.Value.ClientId,
                    ClientSecret = options.Value.ClientSecret,
                },
                Scopes = [GmailService.Scope.GmailSend],
            }),
            "user-gmail",
            tokenResponse
            );

            await creds.RefreshTokenAsync(CancellationToken.None);
            GmailService service = new(new BaseClientService.Initializer
            {
                HttpClientInitializer = creds,
                ApplicationName = "Orchid Backend Gmail",
            });

            MimeMessage message = new();
            message.From.Add(new MailboxAddress("Orchid Lab", options.Value.Email));
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = body };

            using var ms = new MemoryStream();
            await message.WriteToAsync(ms);
            var rawMessage = Convert.ToBase64String(ms.ToArray())
                .Replace("+", "-").Replace("/", "_").Replace("=", "");

            var gmailMessage = new Google.Apis.Gmail.v1.Data.Message { Raw = rawMessage };
            await service.Users.Messages.Send(gmailMessage, options.Value.Email).ExecuteAsync();
        }
    }
}
