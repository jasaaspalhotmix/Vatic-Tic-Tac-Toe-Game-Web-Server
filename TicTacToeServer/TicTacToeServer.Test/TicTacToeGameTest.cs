using System.Collections.Generic;
using System.Xml;
using Microsoft.FSharp.Collections;
using TicTacToeServer.Core;
using TicTacToe.Core;
using Xunit;

namespace TicTacToeServer.Test
{
    public class TicTacToeGameTest
    {
        [Fact]
        public void Make_Game_Not_Null()
        {
            Assert.NotNull(new TicTacToeGame(
                new MockUser(), 
                new MockAi(),
                new GameSettings.gameSetting(3, "x", "@"
                , (int)PlayerValues.playerVals.Human
                , false, false, false)
                ));
        }

        [Fact]
        public void User_Moves()
        {
            var correctOutPut = new List<string>()
                {
                    "x", "@", "x",
                    "x", "@", "x",
                    "@", "x", "@"
                };

            var ticTacToeBox = new List<string>()
                {
                    "-1-", "@", "x",
                    "x", "@", "x",
                    "@", "x", "@"
                };

            var user = new User();
            var aI = new MockAi();
            var game = new TicTacToeGame(
                user,
                aI,
                new GameSettings.gameSetting(3, "x", "@"
                , (int)PlayerValues.playerVals.Human
                , false, false, false)
                );

            var outputBox = game.Play(new TicTacToeBoxClass.TicTacToeBox(
                ListModule.OfSeq(ticTacToeBox)), 1);

            for (var i = 0; i < outputBox.cellCount(); i++)
                Assert.Equal(correctOutPut[i], outputBox.getGlyphAtLocation(i));
        }

        [Fact]
        public void Ai_Moves()
        {
            var correctOutPut = new List<string>()
                {
                    "@", "@", "x",
                    "x", "@", "x",
                    "@", "x", "@"
                };

            var ticTacToeBox = new List<string>()
                {
                    "-1-", "@", "x",
                    "x", "@", "x",
                    "@", "x", "@"
                };

            var user = new MockUser().StubMove(
                    new TicTacToeBoxClass.TicTacToeBox(
                        ListModule.OfSeq(ticTacToeBox)));
            var aI = new Ai();
            var game = new TicTacToeGame(
                user,
                aI,
                new GameSettings.gameSetting(3, "x", "@"
                , (int)PlayerValues.playerVals.Human
                , false, false, false)
                );

            var outputBox = game.Play(new TicTacToeBoxClass.TicTacToeBox(
                ListModule.OfSeq(ticTacToeBox)), 1);

            for (var i = 0; i < outputBox.cellCount(); i++)
                Assert.Equal(correctOutPut[i], outputBox.getGlyphAtLocation(i));
        }

        [Fact]
        public void Ai_Or_User_Does_Not_Move()
        {
            var ticTacToeBox = new List<string>()
            {
                "x", "@", "x",
                "x", "@", "x",
                "@", "x", "@"
            };

            var user = new User();
            var aI = new Ai();
            var game = new TicTacToeGame(
                user,
                aI,
                new GameSettings.gameSetting(3, "@", "x"
                , (int)PlayerValues.playerVals.Human
                , false, false, false)
                );

            var outputBox = game.Play(new TicTacToeBoxClass.TicTacToeBox(
                ListModule.OfSeq(ticTacToeBox)), 1);

            for (var i = 0; i < outputBox.cellCount(); i++)
                Assert.Equal(ticTacToeBox[i], outputBox.getGlyphAtLocation(i));
        }

        [Fact]
        public void User_Wins()
        {
            var ticTacToeBox = new List<string>()
            {
                "x", "@", "x",
                "x", "@", "x",
                "x", "x", "@"
            };

            var user = new User();
            var aI = new Ai();
            var game = new TicTacToeGame(
                user,
                aI,
                new GameSettings.gameSetting(3, "@", "x"
                , (int)PlayerValues.playerVals.Human
                , false, false, false)
                );

            var outputBox = game.CheckForWinner(
                new TicTacToeBoxClass.TicTacToeBox(
                ListModule.OfSeq(ticTacToeBox)));
            Assert.True(outputBox);
        }

        [Fact]
        public void AI_Wins()
        {
            var ticTacToeBox = new List<string>()
            {
                "x", "@", "x",
                "x", "@", "x",
                "@", "@", "@"
            };

            var user = new User();
            var aI = new Ai();
            var game = new TicTacToeGame(
                user,
                aI,
                new GameSettings.gameSetting(3, "@", "x"
                , (int)PlayerValues.playerVals.Human
                , false, false, false)
                );

            var outputBox = game.CheckForWinner(
                new TicTacToeBoxClass.TicTacToeBox(
                ListModule.OfSeq(ticTacToeBox)));

            Assert.True(outputBox);
        }

        [Fact]
        public void Tie_Wins()
        {
            var ticTacToeBox = new List<string>()
            {
                "x", "@", "x",
                "x", "@", "x",
                "@", "x", "@"
            };

            var user = new User();
            var aI = new Ai();
            var game = new TicTacToeGame(
                user,
                aI,
                new GameSettings.gameSetting(3, "@", "x"
                , (int)PlayerValues.playerVals.Human
                , false, false, false)
                );

            var outputBox = game.CheckForWinner(
                new TicTacToeBoxClass.TicTacToeBox(
                ListModule.OfSeq(ticTacToeBox)));
            Assert.True(outputBox);
        }

    }
}