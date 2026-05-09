using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

public class FirebaseService
{
    public FirebaseService()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("firebase-service-account.json"),
            });
        }
    }

    public async Task SendNotificationAsync(string fcmToken, string title, string body)
    {
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