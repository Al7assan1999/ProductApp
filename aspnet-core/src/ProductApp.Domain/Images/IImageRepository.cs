using ProductApp.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace ProductApp.Images
{
    public interface IImageRepository : IRepository<Image, Guid>
    {

    }
}
