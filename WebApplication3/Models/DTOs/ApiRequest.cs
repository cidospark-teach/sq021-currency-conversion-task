namespace WebApplication3.Models.DTOs
{
    public class ApiRequest
    {
        public string Url { get; set; }
        public string ApiType { get; set; }
        public string Endpoint { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
        public Dictionary<string, string> QueryParams { get; set; }

        public ApiRequest()
        {
            QueryParams = new Dictionary<string, string>();
        }
    }
}
