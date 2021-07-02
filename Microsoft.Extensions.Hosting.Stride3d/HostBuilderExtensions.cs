using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Stride;
using Stride.Engine;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseStrideLifetime(this IHostBuilder hostBuilder, Game game)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddSingleton<IHostLifetime>(new StrideLifetime(game)));
        }

        public static IHostBuilder UseStrideLifetime<TGame>(this IHostBuilder hostBuilder)
            where TGame : Game
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddSingleton<IHostLifetime, StrideLifetime<TGame>>());
        }

        public static Task RunStrideAsync(this IHostBuilder hostBuilder, Game game, CancellationToken cancellationToken = default)
        {
            return hostBuilder.UseStrideLifetime(game).Build().RunAsync(cancellationToken);
        }

        public static Task RunStrideAsync<TGame>(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default)
            where TGame : Game
        {
            return hostBuilder.UseStrideLifetime<TGame>().Build().RunAsync(cancellationToken);
        }
    }
}
