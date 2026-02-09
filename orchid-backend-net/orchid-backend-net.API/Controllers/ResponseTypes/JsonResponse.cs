namespace orchid_backend_net.API.Controllers.ResponseTypes
{
    /// <summary>
    /// json response type for api responses
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    public class JsonResponse<T>(T value)
    {
        /// <summary>
        /// value by pass as generic
        /// </summary>
        public T Value { get; set; } = value;
    }
}
