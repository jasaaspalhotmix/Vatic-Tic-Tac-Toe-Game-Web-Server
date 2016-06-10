using TicTacToe.Core;

namespace TicTacToeServer.Core
{
    public class TicTacToeGame
    {
        private readonly IAi _aI;
        public readonly GameSettings.gameSetting Setting;
        private readonly IUser _user;

        public TicTacToeGame(IUser user, IAi aI,
            GameSettings.gameSetting setting)
        {
            _user = user;
            _aI = aI;
            Setting = setting;
        }

        public ITicTacToeBoxClass.ITicTacToeBox Play(
            ITicTacToeBoxClass.ITicTacToeBox ticTacToeBox
            , int move)
        {
            if (CheckForWinner((TicTacToeBoxClass.TicTacToeBox) ticTacToeBox))
                return ticTacToeBox;
            var newTicTacToeBox =
                (TicTacToeBoxClass.TicTacToeBox)
                    _user.Move(ticTacToeBox, move,
                        Setting.playerGlyph, Setting.aIGlyph);
            return !CheckForWinner(newTicTacToeBox)
                ? _aI.Move(newTicTacToeBox, Setting)
                : newTicTacToeBox;
        }

        public bool CheckForWinner(TicTacToeBoxClass.TicTacToeBox ticTacToeBox)
        {
            return CheckForWinnerOrTie.checkForWinnerOrTie(ticTacToeBox,
                Setting.playerGlyph, Setting.aIGlyph)
                   != (int) GameStatusCodes.GenResult.NoWinner;
        }
    }
}