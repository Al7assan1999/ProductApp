using ProductApp.Attributes;
using ProductApp.BlobImage;
using ProductApp.Images;
using ProductApp.LocalizedProducts;
using ProductApp.Variants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace ProductApp.Products
{
    public class ProductAppService :
        CrudAppService<
            Product, 
            ProductDto, 
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateProductDto>,
        IProductAppService
    {
        private readonly IRepository<ProductAttribute, Guid> _attributeRepository;
        private readonly IImageRepository _imageRepository;
        public ProductAppService(IProductRepository repository, IRepository<ProductAttribute, Guid> attributeRepository, IImageRepository imageRepository)
        : base(repository)
        {
            _attributeRepository = attributeRepository;
            _imageRepository = imageRepository;
        }
        protected override async Task<IQueryable<Product>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            return await Repository.WithDetailsAsync();
        }
        
        public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            var errorResult = new ProductDto();
            var product = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);
            var attributes = (await _attributeRepository.WithDetailsAsync()).Where(x => input.Attributes.Contains(x.Id)).ToList();
            //Check if Attributes are valid
            if (attributes.Count != input.Attributes.Count)
            {
                errorResult.ErrorMessage = "Not all Attribute are valid";
                return errorResult;
            }
            //Check if Varints are related to Attributes
            var validVariants = attributes.SelectMany(att => att.Variants).Select(var => var.Id).ToList();
            if (input.Variants.Any(var => !validVariants.Contains(var.Id)))
            {
                errorResult.ErrorMessage = "Not all Variants are valid";
                return errorResult;
            }
            foreach(var variant in input.Variants)
            {
                var updated = attributes.SelectMany(att => att.Variants).FirstOrDefault(var => var.Id == variant.Id);
                updated.ImageId = variant.Id;
            }
            product.Attributes = attributes;
            
            var vImages = input.Variants.Select(x => x.Image).ToList();
            vImages.AddRange(input.Images);
            var images = (await _imageRepository.GetQueryableAsync()).Where(image => vImages.Contains(image.Id)).ToList();
            product.Images = images;
            var localizedProducts = ObjectMapper.Map<List<CreateUpdateLocalizedProductDto>, List<LocalizedProduct>>(input.LocalizedProducts);
            product.LocalizedProducts = localizedProducts;
            var result = await Repository.InsertAsync(product, true);
            var newVariants = new List<Variant>();
            return ObjectMapper.Map<Product, ProductDto>(result);
        }

        public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
        {
            var product = await Repository.GetAsync(id);
            product.Update(input.Name, input.Code, input.Price, input.Description);
            var attributes = (await _attributeRepository.GetQueryableAsync()).Where(x => input.Attributes.Contains(x.Id)).ToList();
            product.Attributes = attributes;
            var result = await Repository.UpdateAsync(product);
            return ObjectMapper.Map<Product, ProductDto>(result);
        }
    }
}
