using System.Collections.Generic;
using Microsoft.FSharp.Collections;
using TicTacToe.Core;
using TicTacToeServer.Core;
using Xunit;

namespace TicTacToeServer.Test
{
    public class AiTest
    {
        [Fact]
        public void Make_Not_Null_Class()
        {
            Assert.NotNull(new Ai());
        }
        [Fact]
        public void Ai_Moves()
        {
            var settings = new GameSettings.gameSetting(3, "x", "@"
                , (int) PlayerValues.playerVals.Human
                , false, false, false);
            var ai = new Ai();
            var correctTicTacToeBox = new TicTacToeBoxClass.TicTacToeBox(
                ListModule.OfSeq(new List<string>()
            {
                "x",
                "x",
                "x",
                "x",
                "x",
                "x",
                "x",
                "x",
                "@"
            }));
            var ticTacToeBox = ai.Move(
                new TicTacToeBoxClass.TicTacToeBox(ListModule.OfSeq(new List<string>()
            {
                "x",
                "x",
                "x",
                "x",
                "x",
                "x",
                "x",
                "x",
                "-9-"
            }))
                , settings);
            for (var i = 0; i < correctTicTacToeBox.cellCount(); i++)
                Assert.Equal(correctTicTacToeBox.getGlyphAtLocation(i)
                    , ticTacToeBox.getGlyphAtLocation(i));
        }
    }
}