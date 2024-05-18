using AutoMapper;
using ProductApi.Models;
using Shared.Dtos;

namespace ProductApi.Infrastructure
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
                {
                    config.CreateMap<ProductDto, Product>().ReverseMap();
                }
            );
            return mapperConfig;
        }
    }
}
