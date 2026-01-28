namespace orchid_backend_net.Application.Common.Interfaces
{
    public interface IImageUploaderService
    {
        Task<string> UpdloadImageAsync(byte[] imageBytes, string fileName, string? folder = null, CancellationToken cancellationToken = default);
    }
}
