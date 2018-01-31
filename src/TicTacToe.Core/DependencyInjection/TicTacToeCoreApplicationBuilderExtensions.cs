using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Core;

namespace Microsoft.AspNetCore.Builder
{
    public static class TicTacToeCoreApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTicTacToeCore(this IApplicationBuilder app)
        {
            var strategy = app.ApplicationServices.GetRequiredService<IGameStrategy>();
            strategy.Calculate();

            return app;
        }
    }
}
