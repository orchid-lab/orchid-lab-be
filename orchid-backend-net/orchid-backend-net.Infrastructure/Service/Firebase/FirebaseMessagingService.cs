using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.Infrastructure.Service
{
    public class FirebaseMessagingService : IFirebaseMessagingService
    {
        private readonly FirebaseMessaging? _messaging;

        public FirebaseMessagingService(IConfiguration configuration)
        {
            var credPath = configuration["Firebase:ServiceAccountPath"];

            // Local không có Firebase thì bỏ qua
            if (string.IsNullOrWhiteSpace(credPath) || !File.Exists(credPath))
            {
                Console.WriteLine("Warning: Firebase not configured, push notifications disabled.");
                _messaging = null;
                return;
            }

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
            if (_messaging == null) return; // Firebase không có thì bỏ qua

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
            if (_messaging == null) return; // Firebase không có thì bỏ qua

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
            await _messaging.SendAllAsync(messages, cancellationToken).ConfigureAwait(false);
        }
    }
}