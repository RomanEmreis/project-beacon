using Beacon.Application;
using Beacon.Application.Providers.Calls;
using Beacon.Application.Providers.Users;
using Beacon.Infrastructure.Providers.Calls;
using Beacon.Infrastructure.Providers.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Beacon.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBeacon(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(config => 
                config.RegisterServicesFromAssemblyContaining<IConnectionHub>());

            serviceCollection
                .AddSingleton<IUsersProvider, UsersProvider>()
                .AddSingleton<ICallsProvider, CallsProvider>();

            return serviceCollection;
        }
    }
}
