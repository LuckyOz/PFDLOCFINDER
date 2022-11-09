
namespace PFDLOCFINDER.Models
{
    public class Response<T>
    {
        public string response_code { get; set; }
        public string response_message { get; set; }
        public int http_response { get; set; }
        public T data { get; set; }
    }
}