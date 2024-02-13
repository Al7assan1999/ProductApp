using ProductApp.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductApp.LocalizedProducts
{
    public class CreateUpdateLocalizedProductDto
    {
        public Language Language { get; set; }
        public string Name { get; set; }
    }
}
