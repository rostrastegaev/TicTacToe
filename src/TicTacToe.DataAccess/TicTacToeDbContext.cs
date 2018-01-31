using Microsoft.EntityFrameworkCore;
using TicTacToe.DataAccess.Configurations;
using TicTacToe.Entities;

namespace TicTacToe.DataAccess
{
    public class TicTacToeDbContext : DbContext
    {
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<TurnEntity> Turns { get; set; }

        public TicTacToeDbContext(DbContextOptions<TicTacToeDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TurnEntityConfiguration());
        }
    }
}
