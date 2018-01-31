namespace TicTacToe.Core
{
    public interface IGameEndingRule
    {
        IGameState Check(Cell cell);
    }
}
