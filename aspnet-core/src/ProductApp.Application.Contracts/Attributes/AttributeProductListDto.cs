using ProductApp.Variants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductApp.Attributes
{
    public class AttributeProductListDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<VariantDto> Variants { get; set; }
    }
}
