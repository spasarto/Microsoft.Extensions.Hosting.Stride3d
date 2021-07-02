using Stride.Engine;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting.Stride
{
    public class StrideLifetime<TGame> : IHostLifetime
        where TGame : Game
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly TGame _game;

        public StrideLifetime(TGame game, IHostApplicationLifetime applicationLifetime)
        {
            _game = game;
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
                _game.Run();
            });

            return Task.CompletedTask;
        }
    }
}
