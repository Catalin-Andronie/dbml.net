using Microsoft.Extensions.DependencyInjection;

namespace DbmlNet.Web.Application;

public static class DbmlNetDependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        return services;
    }
}
