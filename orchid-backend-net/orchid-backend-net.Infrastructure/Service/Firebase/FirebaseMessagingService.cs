using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.Infrastructure.Service
{
    public class FirebaseMessagingService : IFirebaseMessagingService
    {
        private readonly FirebaseMessaging _messaging;

        public FirebaseMessagingService(IConfiguration configuration)
        {
            // Expect configuration key: "Firebase:ServiceAccountPath" (absolute path or env var)
            var credPath = configuration["Firebase:ServiceAccountPath"];
            if (string.IsNullOrWhiteSpace(credPath))
                throw new InvalidOperationException("Firebase:ServiceAccountPath is not configured.");

            // Only create app once
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(credPath)
                });
            }

            _messaging = FirebaseMessaging.DefaultInstance;
        }

        public async Task SendToTokenAsync(string token, string title, string body, CancellationToken cancellationToken = default)
        {
            var message = new Message
            {
                Token = token,
                Notification = new Notification { Title = title, Body = body },
                Android = new AndroidConfig { Priority = Priority.High },
                Apns = new ApnsConfig
                {
                    Headers = new Dictionary<string, string>
                    {
                        ["apns-priority"] = "10"
                    }
                }
            };

            await _messaging.SendAsync(message, cancellationToken).ConfigureAwait(false);
        }

        public async Task SendToTokensAsync(IEnumerable<string> tokens, string title, string body, CancellationToken cancellationToken = default)
        {
            var messages = tokens.Select(t => new Message
            {
                Token = t,
                Notification = new Notification { Title = title, Body = body },
                Android = new AndroidConfig { Priority = Priority.High },
                Apns = new ApnsConfig
                {
                    Headers = new Dictionary<string, string>
                    {
                        ["apns-priority"] = "10"
                    }
                }
            }).ToList();

            if (!messages.Any()) return;

            // SendAllAsync sends a batch
            await _messaging.SendAllAsync(messages, cancellationToken).ConfigureAwait(false);
        }
    }
}