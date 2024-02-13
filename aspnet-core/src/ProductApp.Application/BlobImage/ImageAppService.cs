using ProductApp.Images;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace ProductApp.BlobImage
{
    public class ImageAppService : ApplicationService, IImageAppService
    {
        private readonly IBlobContainer<ProductImageBlob> _blobContainer;
        private readonly IRepository<Image, Guid> _imageRepository;
        public ImageAppService(IBlobContainer<ProductImageBlob> blobContainer, IRepository<Image, Guid> imageRepository)
        {
            _blobContainer = blobContainer;
            _imageRepository = imageRepository;
        }

        public async Task SaveImageAsync(IRemoteStreamContent content)
        {
            var image = new Image()
            {
                Path = Path.GetExtension(content.FileName),
                AltName = content.FileName
            };
            image.Path = Guid.NewGuid() + content.FileName;
            await _blobContainer.SaveAsync(image.Path, content.GetStream(), true);
            
            var resutl = await _imageRepository.InsertAsync(image, true);

        }
    }
}
