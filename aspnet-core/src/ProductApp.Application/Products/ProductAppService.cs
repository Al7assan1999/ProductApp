using ProductApp.Attributes;
using ProductApp.BlobImage;
using ProductApp.Images;
using ProductApp.LocalizedProducts;
using ProductApp.Variants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
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
        protected override async Task<ProductDto> MapToGetListOutputDtoAsync(Product entity)
        {
            var mainImagePath = (await _imageRepository.FirstOrDefaultAsync(x => x.Id == entity.MainImage)).Path;
            var result = ObjectMapper.Map<Product, ProductDto>(entity);
            result.MainImagePath = mainImagePath;
            return result;
        }
        
        public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            var product = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);
            await ValidateProductRequest(input);
            product = await CreateNewProduct(input, product);
            var result = await Repository.InsertAsync(product, true);
            return await MapToGetListOutputDtoAsync(result);
        }
        public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
        {
            var product = await Repository.GetAsync(id);
            if (product is null)
                throw new UserFriendlyException("This Record doesn't exist");
            product = await CreateNewProduct(input, product);
            product.Update(input.Name, input.Code, input.Price, input.Description);
            var result = await Repository.UpdateAsync(product, true);
            return await MapToGetListOutputDtoAsync(result);
        }

        #region Helpers
        private async Task ValidateProductRequest(CreateUpdateProductDto input)
        {
            //Check if Attributes are valid
            if ((await _attributeRepository.WithDetailsAsync()).Where(x => input.Attributes.Select(att => att.Id).Contains(x.Id)).Distinct().Count() != input.Attributes.Count)
            {
                throw new UserFriendlyException("Not all Attribute are valid");
            }
            //Check if Varints are related to Attributes
            var varInoutCount = input.Attributes.SelectMany(at => at.Variants).Select(var => var.Id).Distinct().Count();
            var repoInputCount = (await _attributeRepository.WithDetailsAsync()).AsEnumerable().SelectMany(at => at.Variants).Where(var => input.Attributes.SelectMany(a => a.Variants).Select(v => v.Id).Contains(var.Id)).Distinct().Count();
            if (repoInputCount != varInoutCount)
            {
                throw new UserFriendlyException("Not all Variants are valid");
            }

            //Check if Images are valid
            var vImages = input.Attributes.SelectMany(att => att.Variants).Select(x => x.Image).ToList();
            vImages.AddRange(input.Images);

            if ((await _imageRepository.GetQueryableAsync()).Where(image => vImages.Contains(image.Id)).Distinct().Count() != vImages.Count)
                throw new UserFriendlyException("Not all Images are valid");
        }
        private async Task<Product> CreateNewProduct(CreateUpdateProductDto input, Product product)
        {

            var attributes = (await _attributeRepository.WithDetailsAsync()).Where(x => input.Attributes.Select(att => att.Id).Contains(x.Id)).ToList();
            var vImages = input.Attributes.SelectMany(att => att.Variants).Select(x => x.Image).ToList();
            vImages.AddRange(input.Images);
            var images = (await _imageRepository.GetQueryableAsync()).Where(image => vImages.Contains(image.Id)).ToList();
            List<ProductAttribute> finalAttributes = new List<ProductAttribute>();
            foreach (var att in input.Attributes)
            {
                var variantsWithImages = new List<Variant>();
                var repoAtt = attributes.FirstOrDefault(attr => attr.Id == att.Id);
                foreach (var variant in att.Variants)
                {
                    var repoVariant = attributes.SelectMany(att => att.Variants).FirstOrDefault(var => var.Id == variant.Id);
                    repoVariant.Image = images.FirstOrDefault(x => x.Id == variant.Image);
                    variantsWithImages.Add(repoVariant);
                }
                repoAtt.Variants = variantsWithImages;
                finalAttributes.Add(repoAtt);
            }
            product.Attributes = finalAttributes;

            product.Images = images;
            if (await _imageRepository.AnyAsync(x => x.Id == input.MainImage))
                product.MainImage = input.MainImage;
            else
                throw new UserFriendlyException("Main Image doesn't exist");
            var localizedProducts = ObjectMapper.Map<List<CreateUpdateLocalizedProductDto>, List<LocalizedProduct>>(input.LocalizedProducts);
            product.LocalizedProducts = localizedProducts;
            return product;
        }
        #endregion
    }
}
