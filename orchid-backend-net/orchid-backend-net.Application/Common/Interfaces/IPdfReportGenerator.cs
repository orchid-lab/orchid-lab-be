namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface IPdfReportGenerator
    {
        Task<byte[]> GenerateAsync(object model);
    }
}
