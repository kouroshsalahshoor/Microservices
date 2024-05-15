namespace Shared.Front
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.Get;
        public string Url { get; set; } = string.Empty;
        public object? Data { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
