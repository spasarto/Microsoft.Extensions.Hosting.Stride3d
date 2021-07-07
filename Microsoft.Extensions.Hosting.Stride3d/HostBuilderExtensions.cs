using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Stride;
using Stride.Engine;
using Stride.Games;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseStrideLifetime(this IHostBuilder hostBuilder, GameContext gameContext = null)
        {
            return hostBuilder.ConfigureServices((context, collection) =>
            {
                collection.AddSingleton<Game, Game>();
                collection.AddSingleton<IHostLifetime, StrideLifetime<Game>>();
                collection.AddSingleton(new GameContextProvider(gameContext));
            });
        }

        public static Task RunStrideAsync(this IHostBuilder hostBuilder, GameContext gameContext = null, CancellationToken cancellationToken = default)
        {
            return hostBuilder.UseStrideLifetime(gameContext).Build().RunAsync(cancellationToken);
        }

    }
}
