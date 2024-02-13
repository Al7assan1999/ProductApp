using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Content;

namespace ProductApp.BlobImage
{
    public class CreateUpdateImageDto
    {
        public string Path { get; set; }
        public string AltName { get; set; }
        public IRemoteStreamContent Content { get; set; }
    }
}
