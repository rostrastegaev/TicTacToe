using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Entities;

namespace TicTacToe.DataAccess.Configurations
{
    internal class TurnEntityConfiguration : IEntityTypeConfiguration<TurnEntity>
    {
        public void Configure(EntityTypeBuilder<TurnEntity> builder)
        {
            builder.ToTable("tbTurn");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ForSqlServerUseSequenceHiLo("Turn_Id_HiLo");

            builder.HasOne(t => t.Game)
                .WithMany(g => g.Turns)
                .HasForeignKey(t => t.GameId);
        }
    }
}
