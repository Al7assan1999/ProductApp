using ProductApp.Products;
using ProductApp.Variants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ProductApp.Attributes
{
    public class AttributeAppService : 
        CrudAppService<
            ProductAttribute, //The ProductAttribute entity
            AttributeDto, //Used to show ProductAttributes
            Guid, //Primary key of the ProductAttribute entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateAttributeDto>, //Used to create/update a ProductAttribute
        IAttributeAppService //implement the IProductAttributeAppService
    {
        public AttributeAppService(IAttributeRepository repository)
        : base(repository)
        {

        }
        protected override async Task<IQueryable<ProductAttribute>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            return await Repository.WithDetailsAsync();
        }
        public override async Task<AttributeDto> CreateAsync(CreateUpdateAttributeDto input)
        {
            var attribute = new ProductAttribute()
            {
                Name = input.Name,
                Description = input.Description,
            };
            attribute.Variants = ObjectMapper.Map< List<VariantDto>, List<Variant>>(input.Variants);
            var result = await Repository.InsertAsync(attribute);
            return ObjectMapper.Map<ProductAttribute, AttributeDto>(result);
        }
    }
}
