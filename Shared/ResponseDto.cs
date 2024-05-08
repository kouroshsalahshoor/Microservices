namespace Shared
{
    public class ResponseDto
    {
        public bool IsSuccessful { get; set; }
        public object? Result { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
