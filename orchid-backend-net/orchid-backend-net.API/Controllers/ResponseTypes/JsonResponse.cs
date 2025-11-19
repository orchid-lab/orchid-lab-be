namespace orchid_backend_net.API.Controllers.ResponseTypes
{
    public class JsonResponse<T>(T value)
    {
        public T Value { get; set; } = value;
    }
}
