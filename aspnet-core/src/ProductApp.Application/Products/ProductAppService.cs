using ProductApp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
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
        public ProductAppService(IProductRepository repository, IRepository<ProductAttribute, Guid> attributeRepository)
        : base(repository)
        {
            _attributeRepository = attributeRepository;
        }
        protected override async Task<IQueryable<Product>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            return await Repository.WithDetailsAsync();
        }
        public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            var product = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);
            var attributes = (await _attributeRepository.GetQueryableAsync()).Where(x => input.Attributes.Contains(x.Id)).ToList();
            product.Attributes = attributes;
            var result = await Repository.InsertAsync(product);
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
