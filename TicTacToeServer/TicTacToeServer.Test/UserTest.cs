using System.Collections.Generic;
using Microsoft.FSharp.Collections;
using TicTacToe.Core;
using TicTacToeServer.Core;
using Xunit;

namespace TicTacToeServer.Test
{
    public class UserTest
    {
        [Fact]
        public void User_Class_Is_Not_Null()
        {
            Assert.NotNull(new User());
        }

        [Fact]
        public void User_Moves()
        {
            var user = new User();
            var correctTicTacToeBox = new TicTacToeBoxClass.TicTacToeBox(
                ListModule.OfSeq(new List<string>()
            {
                "x",
                "-2-",
                "-3-",
                "-4-",
                "-5-",
                "-6-",
                "-7-",
                "-8-",
                "-9-"
            }));
            var ticTacToeBox = user.Move(
                new TicTacToeBoxClass.TicTacToeBox(Game.makeTicTacToeBox(3))
                , 1, "x", "@");
            for(var i = 0; i < correctTicTacToeBox.cellCount(); i++)
                Assert.Equal(correctTicTacToeBox.getGlyphAtLocation(i)
                    , ticTacToeBox.getGlyphAtLocation(i));
        }
    }
}