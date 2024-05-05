namespace WebApplication3.Models.DTOs
{
    public class ResponseObject<T>
    {
        public int Code { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
}
