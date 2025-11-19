namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipient, string subject, string body);
    }
}
