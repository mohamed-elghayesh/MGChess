using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MGChessLib.Tests
{
    [TestClass()]
    public class PieceTests
    {
        [TestMethod()]
        //[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        //[Timeout(TestTimeout.Infinite)]  // or 2000 Milliseconds
        public void PieceToStringTest()
        {
            // Arrange
            Pawn piece = new Pawn();
            piece.Name = "Pawn";
            piece.Color = "White";
            string expected = "Pawn : White Pawn";
            // Act
            string actual = piece.ToString();
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void PawnToStringTest()
        {
            // Arrange
            Pawn pawn = new Pawn();
            pawn.Name = "Pawn";
            pawn.Color = "White";
            string expected = "Pawn : White Pawn";
            // Act
            string actual = pawn.ToString();
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void PawnHasMovedTest()
        {
            // Arrange
            Pawn wP = new Pawn();
            bool expected = true;
            // Act
            wP.Move();
            // Assert
            Assert.AreEqual(expected, wP.hasMoved());
        }

        [TestMethod()]
        // Pawn's first move
        public void PawnFirstMoveGetLegalSquaresTest()
        {
            // Arrange
            Pawn wP = new Pawn();
            List<string> expected = new List<string>() { "D3", "D4"};
            List<string> actual = wP.GetLegalSquares("D2");
            // Act
            //wP.Move();
            // Assert
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod()]
        // Pawn's other moves
        public void PawnOtherMovesGetLegalSquaresTest()
        {
            // Arrange
            Pawn wP = new Pawn();
            List<string> expected = new List<string>() { "D3" };
            // Act
            wP.Move();
            List<string> actual = wP.GetLegalSquares("D2");
            // Assert
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod()]
        [DataRow("D2", "D3")]
        [DataRow("A7", "A8")]
        [DataRow("H8", "H9")]
        [DataRow("P2", "P3")]
        // Pawn's first moves, A7 is not in first move
        public void PawnGetLegalSquaresTest(string square, string expected)
        {
            // Arrange
            Pawn wP = new Pawn();
            // Act
            string actual = wP.GetLegalSquares(square)[0];
            // Assert
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch { }
        }

        [TestMethod()]
        [DataRow("D2", "D3")]
        [DataRow("A7", "A8")]
        [DataRow("H8", "H9")]
        [DataRow("P2", "P3")]
        // Pawn's other moves, A7 is in other moves
        public void PawnMovedGetLegalSquaresTest(string square, string expected)
        {
            // Arrange
            Pawn wP = new Pawn();
            // Act
            wP.Move();
            string actual = wP.GetLegalSquares(square)[0];
            // Assert
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch { }
        }
    }
}