using ProductApp.Enums;
using ProductApp.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ProductApp.LocalizedProducts
{
    public class LocalizedProduct : Entity<Guid>
    {
        public Language Language {  get; set; }
        public string Name { get; set; }
        public Product Product { get; set; }
    }
}
