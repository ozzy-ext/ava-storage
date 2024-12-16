using AvaStorage.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AvaStorage.Infrastructure.LocalDisk
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalDiscPictureStorage(this IServiceCollection srv)
        {
            return srv.AddSingleton<IPictureRepository, LocalDiscPictureRepository>();
        }

        public static IServiceCollection ConfigureLocalDiscPictureStorage(this IServiceCollection services, IConfiguration config, string sectionName)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (sectionName == null)
                throw new ArgumentNullException(nameof(sectionName));

            var optionsSection = config.GetSection(sectionName);

            services.Configure<LocalDiskOptions>(optionsSection);

            return services;
        }

        

        public static IServiceCollection ConfigureLocalDiscPictureStorage(this IServiceCollection services, Action<LocalDiskOptions> configureOptions)
        {
            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            services.Configure(configureOptions);

            return services;
        }
    }
}
