using TicTacToe.Core;

namespace TicTacToeServer.Core
{
    public interface IUser
    {
        ITicTacToeBoxClass.ITicTacToeBox Move(
            ITicTacToeBoxClass.ITicTacToeBox ticTacToeBox,
            int move, string playerSymbol, string aISymbol);
    }
}