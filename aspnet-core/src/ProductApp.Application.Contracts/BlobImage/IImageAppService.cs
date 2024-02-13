using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace ProductApp.BlobImage
{
    public interface IImageAppService : IApplicationService
    {
        Task SaveImageAsync(IRemoteStreamContent content);
    }
}
