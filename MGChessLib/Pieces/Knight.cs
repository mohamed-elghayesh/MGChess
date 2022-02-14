using MGChessLib.Common;
using MGChessLib.Squares;

namespace MGChessLib.Pieces
{
    public class Knight : Piece
    {
        private List<Square> validMoves = new List<Square>();

        public Knight(string color) : base(color)
        {
            name = "Knight";
            // using color to set the Image source of the pawn in the constructor
            if (color == Color.Light.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/wN.png", UriKind.Relative)); }
            else if (color == Color.Dark.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/bN.png", UriKind.Relative)); }
        }

        public override List<Square> GetValidMoves(Board.Board board)
        {
            validMoves.Clear();
            validMoves.Add(Square.GetOffsetedSquare(currSquare, -2, -1, board));
            validMoves.Add(Square.GetOffsetedSquare(currSquare, -2, 1, board));
            validMoves.Add(Square.GetOffsetedSquare(currSquare, -1, -2, board));
            validMoves.Add(Square.GetOffsetedSquare(currSquare, -1, 2, board));
            validMoves.Add(Square.GetOffsetedSquare(currSquare, 1, 2, board));
            validMoves.Add(Square.GetOffsetedSquare(currSquare, 1, -2, board));
            validMoves.Add(Square.GetOffsetedSquare(currSquare, 2, 1, board));
            validMoves.Add(Square.GetOffsetedSquare(currSquare, 2, -1, board));

            // remove out of board squares
            validMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");

            // remove the square infront of current square if is occupied
            validMoves = RemoveBlockedSquares(validMoves, board);
            
            return validMoves;
        }

        public override List<Square> RemoveBlockedSquares(List<Square> validMoves, Board.Board board)
        {
            List<Square> validMovesCopy = new List<Square>(validMoves);

            foreach (Square square in validMovesCopy)
            {
                if (square.IsOccupied()) { if (square.GetCurrPiece().GetColor() == this.GetColor()) { validMoves.Remove(square); } }
            }
            return validMoves;
        }
    }
}
