using Microsoft.EntityFrameworkCore;
using ProductApp.EntityFrameworkCore;
using ProductApp.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ProductApp.Product
{
    public class ProductRepository : EfCoreRepository<ProductAppDbContext, Products.Product, Guid>, IProductRepository
    {
        public ProductRepository(IDbContextProvider<ProductAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<Products.Product>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).Include(x => x.Attributes).ThenInclude(x => x.Variants).ThenInclude(x => x.Image).Include(x => x.LocalizedProducts).Include(x => x.Images);
        }
    }
}
