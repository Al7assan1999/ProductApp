using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ProductApp.Attributes
{
    public interface IAttributeAppService : 
        ICrudAppService<
        AttributeDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateAttributeDto> 
    {
    }
}
