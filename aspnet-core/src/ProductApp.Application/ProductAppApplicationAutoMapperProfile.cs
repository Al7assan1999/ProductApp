using AutoMapper;
using ProductApp.Attributes;
using ProductApp.BlobImage;
using ProductApp.Images;
using ProductApp.LocalizedProducts;
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
            .ForMember(x => x.Attributes, opt => opt.Ignore())
            .ForMember(x => x.Images, opt => opt.Ignore());
        CreateMap<ProductAttribute, AttributeDto>().ReverseMap();
        CreateMap<ProductAttribute, AttributeProductListDto>().ReverseMap();
        CreateMap<CreateUpdateAttributeDto, ProductAttribute>();
        CreateMap<Variant, VariantDto>().ReverseMap();
        CreateMap<Image, ImageDto>().ReverseMap();
        CreateMap<CreateUpdateLocalizedProductDto, LocalizedProduct>();
        CreateMap<CreateUpdateImageDto, Image>();
        CreateMap<LocalizedProduct,  LocalizedProductDto>().ReverseMap();
    }
}
