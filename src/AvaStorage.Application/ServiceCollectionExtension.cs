﻿using AvaStorage.Application.Options;
using AvaStorage.Infrastructure.ImageSharp;
using AvaStorage.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AvaStorage.Application;

public static class ServiceCollectionExtension
{
    public const string DefaultConfigSectionName = "AvaStorage";

    public static IServiceCollection AddAvaServiceLogic(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        return services
            .AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Anchor>())
            .AddSingleton<IPictureTools, PictureTools>();
    }

    public static IServiceCollection ConfigureAvaServiceLogic(this IServiceCollection services, IConfiguration config, string sectionName = DefaultConfigSectionName)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config));
        if (sectionName == null)
            throw new ArgumentNullException(nameof(sectionName));

        var optionsSection = config.GetSection(sectionName);

        services.Configure<AvaStorageOptions>(optionsSection);

        return services;
    }

    public static IServiceCollection ConfigureAvaServiceLogic(this IServiceCollection services, Action<AvaStorageOptions> configureOptions)
    {
        if (configureOptions == null)
            throw new ArgumentNullException(nameof(configureOptions));

        services.Configure(configureOptions);

        return services;
    }
}

sealed class Anchor { }