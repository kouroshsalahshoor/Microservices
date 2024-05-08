using AutoMapper;
using CouponApi.Models;
using Shared.Dtos;

namespace CouponApi.Infrastructure
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
                {
                    config.CreateMap<CouponDto, Coupon>().ReverseMap();
                }
            );
            return mapperConfig;
        }
    }
}
