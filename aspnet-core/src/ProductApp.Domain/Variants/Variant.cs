using ProductApp.Attributes;
using ProductApp.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ProductApp.Variants
{
    public class Variant : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ProductAttribute Attribute { get; set; }
        public Guid? ImageId { get; set; }
        public virtual Image? Image { get; set; }

    }
}
