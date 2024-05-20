using CartApi.Services.IServices;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos;

namespace CartApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<List<ProductDto>> Get()
        {
            var client = _clientFactory.CreateClient("Products");
            var response = await client.GetAsync($"");
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto is not null && responseDto.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
            }

            return new List<ProductDto>();
        }
    }
}
