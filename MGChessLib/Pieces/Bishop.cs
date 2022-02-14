using MGChessLib.Common;
using MGChessLib.Squares;

namespace MGChessLib.Pieces
{
    public class Bishop : Piece
    {
        List<Square> validMoves = new List<Square>();

        public Bishop(string color) : base(color)
        {
            name = "Bishop";
            // using color to set the Image source of the pawn in the constructor
            if (color == Color.Light.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/wB.png", UriKind.Relative)); }
            else if (color == Color.Dark.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/bB.png", UriKind.Relative)); }
        }

        public override List<Square> GetValidMoves(Board.Board board)
        {
            validMoves.Clear();
            List<Square> neMoves = new List<Square>(); // north-east
            List<Square> nwMoves = new List<Square>(); // north-west
            List<Square> seMoves = new List<Square>(); // south-east
            List<Square> swMoves = new List<Square>(); // south-west

            // center to the piece location
            for (int i = 0; i < 8; i++)
            {
                seMoves.Add(Square.GetOffsetedSquare(this.GetCurrSquare(), -i, i, board)); 
                neMoves.Add(Square.GetOffsetedSquare(this.GetCurrSquare(), i, i, board)); 
                nwMoves.Add(Square.GetOffsetedSquare(this.GetCurrSquare(), -i, -i, board));
                swMoves.Add(Square.GetOffsetedSquare(this.GetCurrSquare(), i, -i, board)); 
            }

            seMoves.Remove(currSquare);
            neMoves.Remove(currSquare);
            nwMoves.Remove(currSquare);
            swMoves.Remove(currSquare);

            seMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");
            neMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");
            nwMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");
            swMoves.RemoveAll(x => x.GetFile() == "" & x.GetRank() == "");

            // remove blocked and add to validMoves
            validMoves.AddRange(RemoveBlockedSquares(seMoves, board));
            validMoves.AddRange(RemoveBlockedSquares(neMoves, board));
            validMoves.AddRange(RemoveBlockedSquares(nwMoves, board));
            validMoves.AddRange(RemoveBlockedSquares(swMoves, board));

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
