using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRLite.Core.Extensions;

/// <summary>
///  MediatorExtensions class for adding MediatRLite services to the service collection.
/// </summary>
public static class MediatorExtensions
{
    /// <summary>
    ///  Adds MediatRLite services to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddMediatRLite(this IServiceCollection services, params Assembly[] assemblies)
    {
        // Register command and query handlers
        services.AddCommandAndQueryHandlers(assemblies);

        // Register CQRS mediator
        services.AddSingleton<IMediator, Mediator>();

        return services;
    }
}