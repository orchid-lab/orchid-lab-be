using orchid_backend_net.Application.Common.Helper;
using orchid_backend_net.Application.Common.Interfaces;

namespace orchid_backend_net.Infrastructure.Provider
{
    public class VietNamDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => TimeZoneHelper.VietnamTimeNow;

        public bool IsInWorkingHour(DateTime time)
            => TimeZoneHelper.IsInWorkingHour(time);
    }
}
