using Ambev.DeveloperEvaluation.IoC.ModuleInitializers;
using Microsoft.AspNetCore.Builder;

namespace Ambev.DeveloperEvaluation.IoC;
/// <summary>
/// Dependency resolver
/// </summary>
public static class DependencyResolver
{
    /// <summary>
    /// Register dependencies
    /// </summary>
    /// <param name="builder"></param>
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        new ApplicationModuleInitializer().Initialize(builder);
        new InfrastructureModuleInitializer().Initialize(builder);
        new WebApiModuleInitializer().Initialize(builder);
    }
}