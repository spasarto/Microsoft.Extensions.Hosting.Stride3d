using Stride.Engine;
using Stride.Games;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting.Stride
{
    public class StrideLifetime<TGame> : IHostLifetime
        where TGame : Game
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly TGame _game;
        private readonly GameContextProvider _gameContextProvider;

        public StrideLifetime(TGame game, GameContextProvider gameContextProvider, IHostApplicationLifetime applicationLifetime)
        {
            _game = game;
            this._gameContextProvider = gameContextProvider;
            _applicationLifetime = applicationLifetime;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _game.Dispose();
            return Task.CompletedTask;
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            _applicationLifetime.ApplicationStarted.Register(() =>
            {
                _game.WindowCreated += (o, e) =>
                {
                    _game.Window.Closing += (oo, ee) =>
                    {
                        _applicationLifetime.StopApplication();
                    };
                };
                _game.Run(_gameContextProvider.GameContext);
            });

            return Task.CompletedTask;
        }
    }

    public class GameContextProvider
    {
        public GameContext GameContext { get; }

        public GameContextProvider(GameContext gameContext)
        {
            GameContext = gameContext;
        }
    }
}
