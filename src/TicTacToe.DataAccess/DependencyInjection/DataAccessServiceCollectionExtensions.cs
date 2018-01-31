using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TicTacToe.DataAccess;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataAccessServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services,
            IConfigurationSection config)
        {
            services.AddDbContext<TicTacToeDbContext>(builder =>
            {
                builder.UseSqlServer(config["ConnectionString"]);
            });

            return services;
        }
    }
}
