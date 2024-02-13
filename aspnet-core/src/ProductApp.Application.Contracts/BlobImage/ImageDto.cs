using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ProductApp.BlobImage
{
    public class ImageDto : AuditedEntityDto<Guid>
    {
        public string Path { get; set; }
        public string AltName { get; set; }
    }
}
