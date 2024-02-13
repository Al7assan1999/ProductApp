using ProductApp.Variants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductApp.Attributes
{
    public class UpdateAttributeForProduct
    {
        public Guid Id { get; set; }
        public List<CreateUpdateVariantDto> Variants { get; set; }
    }
}
