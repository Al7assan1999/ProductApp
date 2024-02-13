using Microsoft.EntityFrameworkCore;
using ProductApp.Attributes;
using ProductApp.EntityFrameworkCore;
using ProductApp.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ProductApp.Attribute
{
    internal class AttributeRepository : EfCoreRepository<ProductAppDbContext, ProductAttribute, Guid>, IAttributeRepository
    {
        public AttributeRepository(IDbContextProvider<ProductAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<ProductAttribute>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include(x => x.Variants).ThenInclude(x => x.Image);
        }
    }
}
