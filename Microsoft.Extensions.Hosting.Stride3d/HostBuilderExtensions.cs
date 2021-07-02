using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Stride;
using Stride.Engine;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseStrideLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((context, collection) =>
            {
                collection.AddSingleton<Game, Game>();
                collection.AddSingleton<IHostLifetime, StrideLifetime<Game>>();
            });
        }

        public static Task RunStrideAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default)
        {
            return hostBuilder.UseStrideLifetime().Build().RunAsync(cancellationToken);
        }
    }
}
