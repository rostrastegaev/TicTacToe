using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Web.Services;

namespace TicTacToe.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTicTacToeCore();
            services.AddDataAccess(_configuration.GetSection("DataAccess"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentGame, CurrentGame>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ITurnService, TurnService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataAccess.TicTacToeDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseTicTacToeCore();

            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseStaticFiles();
            app.UseMiddleware<GameMiddleware>();
            app.UseMvc();
        }
    }
}
