using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Entities;

namespace TicTacToe.DataAccess.Configurations
{
    internal class GameEntityConfiguration : IEntityTypeConfiguration<GameEntity>
    {
        public void Configure(EntityTypeBuilder<GameEntity> builder)
        {
            builder.ToTable("tbGame");
            builder.HasKey(g => g.Id);
            builder.Property(g => g.Id).ForSqlServerUseSequenceHiLo("Game_Id_HiLo");
        }
    }
}
