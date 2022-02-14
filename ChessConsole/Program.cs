using MGChessLib.Pieces;
using MGChessLib.Board;
using MGChessLib.Squares;
using MGChessLib.Common;

// See https://aka.ms/new-console-template for more information
Board board = new Board();
board.LoadPieces();

// validate the move before moving 
Square sourceSq = board.GetSquare("E", "2");
Square targetSq = board.GetSquare("E", "4");
Movement move = new Movement(sourceSq, targetSq, board);

if (move.IsValidMove(board, out string message))
{
    board.SetMove(move);
    Console.WriteLine(message);
}
else { Console.WriteLine(message); }


//=============================================================//
// Now Pawns can move to capture square if it is empty, revise //
//=============================================================//
Square sourceSq1 = board.GetSquare("D", "7");
Square targetSq1 = board.GetSquare("D", "6");

Movement move1 = new Movement(sourceSq1, targetSq1, board);

if (move1.IsValidMove(board, out string message1))
{
    board.SetMove(move1);
    Console.WriteLine(message1);
}
else { Console.WriteLine(message1); }


board.PrintBoard();




