using TicTacToe.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TicTacToeCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddTicTacToeCore(this IServiceCollection services)
        {
            services.AddSingleton<IGameFactory, GameFactory>();
            services.AddSingleton<IPlayersQueueFactory, PlayersQueueFactory>();
            services.AddSingleton<IGameStrategy, GameStrategy>();

            return services;
        }
    }
}
