using Microsoft.AspNetCore.Builder;

namespace Ambev.DeveloperEvaluation.IoC;
/// <summary>
/// Module initializer interface
/// </summary>
public interface IModuleInitializer
{
    /// <summary>
    /// Initialize the module
    /// </summary>
    /// <param name="builder"></param>
    void Initialize(WebApplicationBuilder builder);
}
