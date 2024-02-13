using ProductApp.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Localization;

namespace ProductApp.LocalizedProducts
{
    public class LocalizedProductDto
    {
        public Language Language { get; set; }
        public string Name { get; set; }
    }
}
