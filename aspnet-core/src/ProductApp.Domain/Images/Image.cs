using ProductApp.Products;
using ProductApp.Variants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ProductApp.Images
{
    public class Image : AuditedAggregateRoot<Guid>
    {
        public string Path { get; set; }
        public string AltName { get; set; }
        public Guid? VariantId { get; set; }
        public Variant? Variant { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

    }
}
