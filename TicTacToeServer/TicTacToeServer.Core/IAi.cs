using TicTacToe.Core;

namespace TicTacToeServer.Core
{
    public interface IAi
    {
        ITicTacToeBoxClass.ITicTacToeBox Move(
            ITicTacToeBoxClass.ITicTacToeBox ticTacToeBox,
            GameSettings.gameSetting settings);
    }
}
