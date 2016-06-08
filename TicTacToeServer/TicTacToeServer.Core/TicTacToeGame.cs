using TicTacToe.Core;

namespace TicTacToeServer.Core
{
    public class TicTacToeGame
    {
        private readonly IAi _aI;
        private readonly GameSettings.gameSetting _setting;
        private readonly IUser _user;

        public TicTacToeGame(IUser user, IAi aI,
            GameSettings.gameSetting setting)
        {
            _user = user;
            _aI = aI;
            _setting = setting;
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
                        _setting.playerGlyph, _setting.aIGlyph);
            return !CheckForWinner(newTicTacToeBox)
                ? _aI.Move(newTicTacToeBox, _setting)
                : newTicTacToeBox;
        }

        public bool CheckForWinner(TicTacToeBoxClass.TicTacToeBox ticTacToeBox)
        {
            return CheckForWinnerOrTie.checkForWinnerOrTie(ticTacToeBox,
                _setting.playerGlyph, _setting.aIGlyph)
                   != (int) GameStatusCodes.GenResult.NoWinner;
        }
    }
}