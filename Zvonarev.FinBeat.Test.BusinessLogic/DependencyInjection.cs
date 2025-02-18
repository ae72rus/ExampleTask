using Microsoft.Extensions.DependencyInjection;

namespace Zvonarev.FinBeat.Test.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddTestFunctions(this IServiceCollection services)
    {
        //todo some custom logic to add here
        return services;
    }
}