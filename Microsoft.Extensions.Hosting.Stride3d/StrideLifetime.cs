using Stride.Engine;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting.Stride
{
    public class StrideLifetime : IHostLifetime
    {
        private readonly Game _game;
        private Thread _gameThread;

        public StrideLifetime(Game game)
        {
            _game = game;
            _game.Exiting += _game_Exiting;
        }
        private void _game_Exiting(object sender, EventArgs e)
        {
            StopAsync(default);
            _gameThread.Interrupt();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _game.Dispose();
            return Task.CompletedTask;
        }

        public async Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(0);
                _gameThread = Thread.CurrentThread;
                _game.Run();
            }
            catch (ThreadInterruptedException) { }
        }
    }


    public class StrideLifetime<TGame> : IHostLifetime
        where TGame : Game
    {
        private readonly TGame _game;

        public StrideLifetime(TGame game)
        {
            _game = game;
            _game.Exiting += _game_Exiting;
        }

        private void _game_Exiting(object sender, EventArgs e)
        {
            StopAsync(default);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _game.Dispose();
            return Task.CompletedTask;
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            _game.Run();
            return Task.CompletedTask;
        }
    }
}
