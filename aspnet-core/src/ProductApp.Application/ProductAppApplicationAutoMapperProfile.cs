using AutoMapper;
using ProductApp.Attributes;
using ProductApp.Products;
using ProductApp.Variants;
using Volo.Abp.AutoMapper;

namespace ProductApp;

public class ProductAppApplicationAutoMapperProfile : Profile
{
    public ProductAppApplicationAutoMapperProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CreateUpdateProductDto, Product>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Attributes, opt => opt.Ignore());
        CreateMap<ProductAttribute, AttributeDto>().ReverseMap();
        CreateMap<CreateUpdateAttributeDto, ProductAttribute>();
        CreateMap<Variant, VariantDto>().ReverseMap();
    }
}
