using AvaStorage.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AvaStorage.Infrastructure.LocalDisk
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalDiscPictureRepository(this IServiceCollection srv)
        {
            return srv.AddSingleton<IPictureRepository, LocalDiscPictureRepository>();
        }
    }
}
