using ProductApp.BlobImage;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace ProductApp;

[DependsOn(
    typeof(ProductAppDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(ProductAppApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpBlobStoringFileSystemModule)
    )]
public class ProductAppApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.Configure<ProductImageBlob>(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = "ProductImages";
                    fileSystem.AppendContainerNameToBasePath = true;
                });
            });
        });
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ProductAppApplicationModule>();
        });
    }
}
