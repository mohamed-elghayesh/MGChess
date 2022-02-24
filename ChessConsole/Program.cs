using MGChessLib.Pieces;
using MGChessLib.Board;
using MGChessLib.Squares;
using MGChessLib.Common;

// See https://aka.ms/new-console-template for more information
Board board = new Board();
board.LoadPieces();

// light colored piece
Movement move1 = new Movement(board.GetSquare("E", "2"), board.GetSquare("E", "4"), board);
board.SetMove(move1, null);
// dark colored piece
Movement move2 = new Movement(board.GetSquare("D", "7"), board.GetSquare("D", "6"), board);
board.SetMove(move2, null);

Movement move3 = new Movement(board.GetSquare("F", "1"), board.GetSquare("C", "4"), board);
board.SetMove(move3, null);

Movement move4 = new Movement(board.GetSquare("B", "8"), board.GetSquare("C", "6"), board);
board.SetMove(move4, null);

Movement move5 = new Movement(board.GetSquare("G", "1"), board.GetSquare("F", "3"), board);
board.SetMove(move5, null);

Movement move6 = new Movement(board.GetSquare("C", "8"), board.GetSquare("F", "5"), board);
board.SetMove(move6, null);

Movement move7 = new Movement(board.GetSquare("E", "1"), board.GetSquare("G", "1"), board);
board.SetMove(move7, null);

board.PrintBoard();




