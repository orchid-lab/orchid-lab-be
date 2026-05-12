using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

public class FirebaseService
{
    public FirebaseService()
    {
        try
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var credPath = "firebase-service-account.json";
                if (!File.Exists(credPath))
                {
                    Console.WriteLine("Warning: firebase-service-account.json not found, push notifications disabled.");
                    return;
                }

                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(credPath),
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Firebase init failed - {ex.Message}");
        }
    }

    public async Task SendNotificationAsync(string fcmToken, string title, string body)
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            Console.WriteLine("Warning: Firebase not initialized, skipping notification.");
            return;
        }

        var message = new FirebaseAdmin.Messaging.Message
        {
            Token = fcmToken,
            Notification = new FirebaseAdmin.Messaging.Notification
            {
                Title = title,
                Body = body,
            },
        };
        await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendAsync(message);
    }
}