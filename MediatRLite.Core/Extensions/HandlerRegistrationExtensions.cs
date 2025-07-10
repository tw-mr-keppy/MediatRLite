using System.Reflection;
using MediatRLite.Core.Commands;
using MediatRLite.Core.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace MediatRLite.Core.Extensions;

/// <summary>
///  HandlerRegistrationExtensions class for registering command and query handlers.
/// </summary>
internal static class HandlerRegistrationExtensions
{
    /// <summary>
    ///  Registers command and query handlers from the specified assemblies.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    internal static IServiceCollection AddCommandAndQueryHandlers(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(a => a.GetTypes()).Where(t => t.IsClass && !t.IsAbstract);

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();

            foreach (var iface in interfaces)
            {
                if (iface.IsGenericType)
                {
                    var genericTypeDef = iface.GetGenericTypeDefinition();

                    if (genericTypeDef == typeof(ICommandHandler<,>) ||
                        genericTypeDef == typeof(IQueryHandler<,>))
                    {
                        services.AddTransient(iface, type);
                    }
                }
            }
        }

        return services;
    }
}