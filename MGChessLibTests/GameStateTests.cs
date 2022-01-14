using Microsoft.VisualStudio.TestTools.UnitTesting;
using MGChessLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGChessLib.Tests
{
    [TestClass()]
    public class GameStateTests
    {
        [TestMethod()]
        public void GetInitialGameBoardTest()
        {
            // Arrange
            GameState gs = new GameState();
            // Act
            Dictionary<string, string> expectedBoard = Board.InitBoard();
            Dictionary<string, string> actualBoard = gs.GetGameBoard();
            // Assert
            Assert.AreSame(expectedBoard, actualBoard);
        }
    }
}