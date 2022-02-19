using MGChessLib.Common;
using MGChessLib.Squares;

namespace MGChessLib.Pieces
{
    public class Rook : Piece
    {
        public bool IsFirstMove = true;
        private List<Square> validMoves = new List<Square>();

        public Rook(string color) : base(color)
        {
            name = "Rook";
            // using color to set the Image source of the pawn in the constructor
            if (color == Color.Light.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/wR.png", UriKind.Relative)); }
            else if (color == Color.Dark.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/bR.png", UriKind.Relative)); }
        }

        public override List<Square> GetValidMoves(Board.Board board)
        {
            validMoves.Clear();
            List<Square> northMoves = new List<Square>();
            List<Square> southMoves = new List<Square>();
            List<Square> westMoves = new List<Square>();
            List<Square> eastMoves = new List<Square>();

            for (int i = 0; i < 8; i++)
            {
                eastMoves.Add(Square.GetOffsetedSquare(this.GetCurrSquare(), i, 0, board)); // one file right
                westMoves.Add(Square.GetOffsetedSquare(this.GetCurrSquare(), -i, 0, board)); // one file left
                northMoves.Add(Square.GetOffsetedSquare(this.GetCurrSquare(), 0, i, board)); // one rank up
                southMoves.Add(Square.GetOffsetedSquare(this.GetCurrSquare(), 0, -i, board)); // one rank down
            }

            // clean temp vectors
            eastMoves.Remove(currSquare);
            westMoves.Remove(currSquare);
            southMoves.Remove(currSquare);
            northMoves.Remove(currSquare);

            eastMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");
            westMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");
            northMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");
            southMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");

            // remove blocked and add to validMoves
            validMoves.AddRange(RemoveBlockedSquares(eastMoves, board));
            validMoves.AddRange(RemoveBlockedSquares(westMoves, board));
            validMoves.AddRange(RemoveBlockedSquares(northMoves, board));
            validMoves.AddRange(RemoveBlockedSquares(southMoves, board));

            return validMoves;
        }

        public override List<Square> RemoveBlockedSquares(List<Square> validMoves, Board.Board board)
        {
            // if an index is occupied remove it and all that are next
            List<Square> result = new List<Square>();
            foreach (Square square in validMoves)
            {
                if (!square.IsOccupied()) { result.Add(square); }
                else // square is occupied
                { 
                    // check color, add if opposite, then break
                    if (this.color != square.GetCurrPiece().GetColor()) { result.Add(square); }
                    break; 
                }
            }
            return result;
        }
    }
}
