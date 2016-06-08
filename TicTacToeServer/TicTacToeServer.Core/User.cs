using TicTacToe.Core;

namespace TicTacToeServer.Core
{
    public class User : IUser
    {
        public ITicTacToeBoxClass.ITicTacToeBox Move(
            ITicTacToeBoxClass.ITicTacToeBox ticTacToeBox, 
            int move, string playerSymbol, string aISymbol)
        {
            return TicTacToeBoxEdit.insertUserOption(ticTacToeBox
                , move, playerSymbol, aISymbol);
        }
    }
}