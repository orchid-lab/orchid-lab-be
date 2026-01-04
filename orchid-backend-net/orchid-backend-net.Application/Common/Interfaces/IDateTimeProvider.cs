namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        bool IsInWorkingHour(DateTime time);
    }
}
