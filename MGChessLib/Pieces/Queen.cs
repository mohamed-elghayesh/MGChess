using MGChessLib.Common;
using MGChessLib.Squares;

namespace MGChessLib.Pieces
{
    public class Queen : Piece
    {
        public Queen(string color) : base(color)
        {
            name = "Queen";            
            // using color to set the Image source of the pawn in the constructor
            if (color == Color.Light.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/wQ.png", UriKind.Relative)); }
            else if (color == Color.Dark.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/bQ.png", UriKind.Relative)); }
        }

        public override List<Square> GetValidMoves(Board.Board board)
        {
            List<Square> validMoves = new List<Square>();

            Piece bishop = new Bishop(this.color);
            bishop.SetCurrSquare(this.currSquare);
            Piece rook = new Rook(this.color);
            rook.SetCurrSquare(this.currSquare);

            validMoves.AddRange(bishop.GetValidMoves(board));
            validMoves.AddRange(rook.GetValidMoves(board));
            validMoves = validMoves.Distinct().ToList();

            return validMoves;
        }

        // no need to implement here
        public override List<Square> RemoveBlockedSquares(List<Square> validMoves, Board.Board board)
        {
            throw new NotImplementedException();
        }
    }
}
