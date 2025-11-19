namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface IImageUploaderService
    {
        Task<string> UpdloadImageAsync(Stream fileStream, string fileName, string? folder = null);
    }
}
