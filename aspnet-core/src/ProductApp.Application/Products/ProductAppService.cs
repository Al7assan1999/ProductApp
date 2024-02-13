﻿using ProductApp.Attributes;
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
            var attributes = (await _attributeRepository.WithDetailsAsync()).Where(x => input.Attributes.Select(att => att.Id).Contains(x.Id)).ToList();
            //Check if Attributes are valid
            if (attributes.Distinct().Count() != input.Attributes.Distinct().Count())
            {
                throw new UserFriendlyException("Not all Attribute are valid"); 
            }
            //Check if Varints are related to Attributes
            var validVariants = attributes.SelectMany(att => att.Variants).Select(var => var.Id).ToList();
            if (input.Attributes.SelectMany(var => var.Variants).Any(var => !validVariants.Contains(var.Id)))
            {
                throw new UserFriendlyException("Not all Variants are valid");
            }

            //Check if Images are valid
            var vImages = input.Attributes.SelectMany(att => att.Variants).Select(x => x.Image).ToList();
            vImages.AddRange(input.Images);
            var images = (await _imageRepository.GetQueryableAsync()).Where(image => vImages.Contains(image.Id)).ToList();
            if (images.Distinct().Count() != vImages.Distinct().Count())
                throw new UserFriendlyException("Not all Images are valid");

            List<ProductAttribute> finalAttributes = new List<ProductAttribute>();
            foreach(var att in input.Attributes)
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
            var result = await Repository.InsertAsync(product, true);
            return await MapToGetListOutputDtoAsync(result);
        }

        public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
        {
            var product = await Repository.GetAsync(id);
            if (product is null)
                throw new UserFriendlyException("This Record doesn't exist");
            product.Update(input.Name, input.Code, input.Price, input.Description);
            var attributes = (await _attributeRepository.WithDetailsAsync()).Where(x => input.Attributes.Select(att => att.Id).Contains(x.Id)).ToList();
            //Check if Attributes are valid
            if (attributes.Count != input.Attributes.Count)
            {
                throw new UserFriendlyException("Not all Attribute are valid");
            }
            //Check if Varints are related to Attributes
            var validVariants = attributes.SelectMany(att => att.Variants).Select(var => var.Id).ToList();
            if (input.Attributes.SelectMany(var => var.Variants).Any(var => !validVariants.Contains(var.Id)))
            {
                throw new UserFriendlyException("Not all Variants are valid");
            }

            //Check if Images are valid
            var vImages = input.Attributes.SelectMany(att => att.Variants).Select(x => x.Image).ToList();
            vImages.AddRange(input.Images);
            var images = (await _imageRepository.GetQueryableAsync()).Where(image => vImages.Contains(image.Id)).ToList();
            if (images.Count != vImages.Count)
                throw new UserFriendlyException("Not all Images are valid");

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
            var result = await Repository.UpdateAsync(product, true);
            return await MapToGetListOutputDtoAsync(result);
        }
    }
}
