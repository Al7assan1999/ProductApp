using ProductApp.Attributes;
using ProductApp.BlobImage;
using ProductApp.LocalizedProducts;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ProductApp.Products
{
    public class ProductDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public ImageDto Image { get; set; }
        public List<AttributeProductListDto> Attributes { get; set; }
        public List<LocalizedProductDto> LocalizedProducts { get; set; }
    }
}
