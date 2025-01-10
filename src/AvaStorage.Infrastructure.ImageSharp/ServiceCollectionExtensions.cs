using AvaStorage.Application.Services;
using AvaStorage.Infrastructure.ImageSharp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AvaStorage.Infrastructure.ImageSharp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAvaStorageImageSharpInfrastructure(this IServiceCollection srv)
        {
            return srv.AddSingleton<IImageMetadataExtractor, ImageSharpImageMetadataExtractor>()
                .AddSingleton<IImageModifier, ImageSharpImageModifier>();
        }
    }
}
