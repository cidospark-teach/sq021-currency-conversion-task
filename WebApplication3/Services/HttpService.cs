using Newtonsoft.Json;
using WebApplication3.Models.DTOs;

namespace WebApplication3.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResponse<T>> MakeRequestAsync<T>(ApiRequest request)
        {
            //using var client = new HttpClient();

            if (request.QueryParams.Any())
            {
                foreach(var kvp in request.QueryParams)
                {
                    _httpClient.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                }
            }

            HttpResponseMessage? res = null;
            switch (request.ApiType)
            {
                case "GET":
                    res = await _httpClient.GetAsync($"{request.Url}{request.Endpoint}");
                break;
                default:
                    return new ApiResponse<T> { Error = "Api type is not supported" };
            }

            if(res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(content);

                return new ApiResponse<T>
                {
                    IsSuccess = true,
                    Data = result
                };
            }

            return new ApiResponse<T>();
        }
    }
}
