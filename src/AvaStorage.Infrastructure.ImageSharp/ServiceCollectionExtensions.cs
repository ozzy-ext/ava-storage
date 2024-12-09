using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaStorage.Application.Services;
using AvaStorage.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AvaStorage.Infrastructure.ImageSharp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAvaStorageImageSharpInfrastructure(this IServiceCollection srv)
        {
            return srv.AddSingleton<IPictureTools, ImageSharpPictureTools>();
        }
    }
}
