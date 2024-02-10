using ProductApp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ProductApp.Products
{
    public class Product : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProductAttribute> Attributes { get; set; }

        public void Update(string Name, string Code, decimal Price, string Description)
        {
            this.Name = Name;
            this.Code = Code;
            this.Price = Price;
            this.Description = Description;

        }
    }
}
