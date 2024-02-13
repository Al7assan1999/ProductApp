using ProductApp.EntityFrameworkCore;
using ProductApp.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ProductApp.Images
{
    public class ImageRepository : EfCoreRepository<ProductAppDbContext, Image, Guid>, IImageRepository
    {
        public ImageRepository(IDbContextProvider<ProductAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

    }
}
