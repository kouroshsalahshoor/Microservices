using CartApi.Services.IServices;
using Newtonsoft.Json;
using Shared;
using Shared.Dtos;

namespace CartApi.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CouponService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<CouponDto> Get(string code)
        {
            var client = _clientFactory.CreateClient("Coupons");
            var response = await client.GetAsync($"code/{code}");
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto is not null && responseDto.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }

            return new CouponDto();
        }
    }
}
