using BlazorAuto.Client.Services.IService;
using Newtonsoft.Json;
using Shared;
using Shared.Front;
using System.Net;
using System.Text;

namespace BlazorAuto.Client.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto dto)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("myClient");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                //token

                message.RequestUri = new Uri(dto.Url);
                if (dto.Data is not null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(dto.Data), Encoding.UTF8, "application.json");
                }

                switch (dto.ApiType)
                {
                    case ApiType.Post:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.Put:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.Delete:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage response = await client.SendAsync(message);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new ResponseDto() { IsSuccessful = false, Errors = new List<string> { "Not Found" } };
                    case HttpStatusCode.Forbidden:
                        return new ResponseDto() { IsSuccessful = false, Errors = new List<string> { "Forbidden - Acces Denied!" } };
                    case HttpStatusCode.Unauthorized:
                        return new ResponseDto() { IsSuccessful = false, Errors = new List<string> { "Unauthorized" } };
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDto() { IsSuccessful = false, Errors = new List<string> { "Internal Server Error" } };
                    default:
                        var apiContent = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto() { IsSuccessful = false, Errors = new List<string> { ex.Message } };
            }
            
        }
    }
}
