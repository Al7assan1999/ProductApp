using ProductApp.BlobImage;
using ProductApp.LocalizedProducts;
using ProductApp.Variants;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace ProductApp.Products
{
    public class CreateUpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public List<Guid> Attributes { get; set; }
        public List<CreateUpdateVariantDto> Variants { get; set; }
        public List<Guid> Images { get; set; }
        public List<CreateUpdateLocalizedProductDto> LocalizedProducts { get; set; }
    }
}
