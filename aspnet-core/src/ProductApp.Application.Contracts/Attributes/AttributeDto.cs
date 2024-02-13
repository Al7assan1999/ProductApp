using ProductApp.Products;
using ProductApp.Variants;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ProductApp.Attributes
{
    public class AttributeDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public List<VariantDto> Variants { get; set; }
    }
}
