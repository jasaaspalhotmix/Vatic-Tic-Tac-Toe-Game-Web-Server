using TicTacToe.Core;

namespace TicTacToeServer.Core
{
    public class Ai : IAi
    {
        public ITicTacToeBoxClass.ITicTacToeBox Move(
            ITicTacToeBoxClass.ITicTacToeBox ticTacToeBox,
            GameSettings.gameSetting settings)
        {
            return AI.aIMove(settings, ticTacToeBox);
        }
    }

}