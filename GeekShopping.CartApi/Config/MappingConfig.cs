using AutoMapper;
using GeekShopping.CartApi.Data.ValueObjects;
using GeekShopping.CartApi.Model;
using GeekShopping.CartAPI.Model;

namespace GeekShopping.CartAPI.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ProductVO, Product>().ReverseMap();
            CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
            CreateMap<CartDetailVO, CartDetail>().ReverseMap();
            CreateMap<CartVO, Cart>().ReverseMap();
        }
    }
}