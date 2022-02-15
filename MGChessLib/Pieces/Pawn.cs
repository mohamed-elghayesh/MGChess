using MGChessLib.Common;
using MGChessLib.Squares;

namespace MGChessLib.Pieces
{
    public class Pawn : Piece
    {
        private bool isFirstMove = true;
        private List<Square> validMoves = new List<Square>();

        public Pawn(string color) : base(color)
        {
            name = "Pawn";
            // using color to set the Image source of the pawn in the constructor
            if (color == Color.Light.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/wP.png", UriKind.Relative)); }
            else if (color == Color.Dark.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/bP.png", UriKind.Relative)); }
        }

        public override List<Square> GetValidMoves(Board.Board board)
        {
            // implement color
            validMoves.Clear();
            // first move or other moves involve moving 1 step forward
            if (color == Color.Light.ToString())
            {
                validMoves.Add(Square.GetOffsetedSquare(currSquare, 0, 1, board));
                if (isFirstMove) { validMoves.Add(Square.GetOffsetedSquare(GetCurrSquare(), 0, 2, board)); }
                // capture squares
                validMoves.Add(Square.GetOffsetedSquare(currSquare, 1, 1, board));
                validMoves.Add(Square.GetOffsetedSquare(currSquare, -1, 1, board));
                // remove the square infront of current square if is occupied
                validMoves = RemoveBlockedSquares(validMoves, board);
            }
            else if (color == Color.Dark.ToString()) // color is dark
            {
                validMoves.Add(Square.GetOffsetedSquare(currSquare, 0, -1, board));
                if (isFirstMove) { validMoves.Add(Square.GetOffsetedSquare(GetCurrSquare(), 0, -2, board)); }
                // capture squares
                validMoves.Add(Square.GetOffsetedSquare(currSquare, 1, -1, board));
                validMoves.Add(Square.GetOffsetedSquare(currSquare, -1, -1, board));
                // remove the square infront of current square if is occupied
                validMoves = RemoveBlockedSquares(validMoves, board);
            }

            return validMoves;
        }
        
        public override List<Square> RemoveBlockedSquares(List<Square> validMoves, Board.Board board)
        {
            Square front1 = (currSquare.GetCurrPiece().GetColor() == Color.Light.ToString()) ?
                                 Square.GetOffsetedSquare(currSquare, 0, 1, board) : Square.GetOffsetedSquare(currSquare, 0, -1, board);
            Square front2 = (currSquare.GetCurrPiece().GetColor() == Color.Light.ToString()) ?
                                 Square.GetOffsetedSquare(currSquare, 0, 2, board) : Square.GetOffsetedSquare(currSquare, 0, -2, board);
            Square capture1 = (currSquare.GetCurrPiece().GetColor() == Color.Light.ToString()) ?
                                 Square.GetOffsetedSquare(currSquare, 1, 1, board) : Square.GetOffsetedSquare(currSquare, 1, -1, board);
            Square capture2 = (currSquare.GetCurrPiece().GetColor() == Color.Light.ToString()) ?
                                 Square.GetOffsetedSquare(currSquare, -1, 1, board) : Square.GetOffsetedSquare(currSquare, -1, -1, board);

            // blocked move or capture
            validMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");

            if (front1.IsOccupied()) { validMoves.Remove(front1); validMoves.Remove(front2); }
            if (front2.IsOccupied()) { validMoves.Remove(front2); }
            if (capture1.IsOccupied()) { if (capture1.GetCurrPiece().GetColor() == this.GetColor()) { validMoves.Remove(capture1); } }
            else { validMoves.Remove(capture1); } // remove if is not occupied
            if (capture2.IsOccupied()) { if (capture2.GetCurrPiece().GetColor() == this.GetColor()) { validMoves.Remove(capture2); } }
            else { validMoves.Remove(capture2); }

            return validMoves;
        }

        public bool IsFirstMove()
        {
            return isFirstMove;
        }
    }
}
