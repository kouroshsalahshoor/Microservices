using BlazorWasm.Services.IServices;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos.Auth;
using Shared.Front;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace BlazorWam.Services
{
    public class BaseService : IBaseService
    {
        //private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;
        private readonly IJSRuntime _js;

        public BaseService(IHttpClientFactory httpClientFactory, IJSRuntime js)
        {
            _client = httpClientFactory.CreateClient("xClient");
            //_httpClientFactory = httpClientFactory;
            _js = js;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto dto)
        {
            try
            {
                //var client = _httpClientFactory.CreateClient("xClient");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                //token
                var token = await _js.InvokeAsync<string>("localStorage.getItem", ApplicationConstants.Local_Token);
                if (string.IsNullOrEmpty(token) == false)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }

                message.RequestUri = new Uri(dto.Url);
                if (dto.Data is not null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(dto.Data), Encoding.UTF8, "application/json");
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

                HttpResponseMessage response = await _client.SendAsync(message);

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
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto() { IsSuccessful = false, Errors = new List<string> { ex.Message } };
            }

        }

        public async Task Logout()
        {
            await _js.InvokeVoidAsync("localStorage.setItem", ApplicationConstants.Local_Token, string.Empty);
            await _js.InvokeVoidAsync("localStorage.setItem", ApplicationConstants.Current_User, string.Empty);
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
