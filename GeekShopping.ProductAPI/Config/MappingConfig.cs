using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model;

namespace GeekShopping.ProductAPI.Config
{
    public class MappingConfig
    {
        public static void ConfigureMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<ProductVO, Product>();
            config.CreateMap<Product, ProductVO>();
        }
    }
}
