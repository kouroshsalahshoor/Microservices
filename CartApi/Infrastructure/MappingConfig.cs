using AutoMapper;
using CartApi.Models;
using Shared.Dtos.Cart;

namespace CartApi.Infrastructure
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
                {
                    config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                    config.CreateMap<CartDetail, CartDetailDto>().ReverseMap();
                }
            );
            return mapperConfig;
        }
    }
}
