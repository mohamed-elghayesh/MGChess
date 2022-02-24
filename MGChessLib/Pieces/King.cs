using MGChessLib.Common;
using MGChessLib.Squares;

namespace MGChessLib.Pieces
{
    public class King : Piece
    {
        public bool IsFirstMove = true;
        public bool IsUnderCheck = false;
        List<Square> validMoves = new List<Square>();

        public King(string color) : base(color)
        {
            name = "King";
            // using color to set the Image source of the pawn in the constructor
            if (color == Color.Light.ToString()) { SetImageSource(new Uri(@"/ChessWPF;component/Images/wK.png", UriKind.Relative)); }
            else if (color == Color.Dark.ToString()) { SetImageSource(imageSource = new Uri(@"/ChessWPF;component/Images/bK.png", UriKind.Relative)); }
        }

        public override List<Square> GetValidMoves(Board.Board board)
        {
            validMoves.Clear();

            Piece queen = new Queen(this.color);
            queen.SetCurrSquare(this.currSquare);

            validMoves.AddRange(queen.GetValidMoves(board));
            validMoves.Remove(currSquare);

            validMoves.RemoveAll(x => Math.Abs(x.GetRankIndex(x.GetRank()) - currSquare.GetRankIndex(currSquare.GetRank())) > 1 ||
                                      Math.Abs(x.GetFileIndex(x.GetFile()) - currSquare.GetFileIndex(currSquare.GetFile())) > 1);
            
            if (IsFirstMove) { validMoves.AddRange(GetCastleSquares(validMoves, board)); }
            
            return validMoves;
        }

        private List<Square> GetCastleSquares(List<Square> moves, Board.Board board)
        {
            if (!Square.GetOffsetedSquare(currSquare, 2, 0, board).IsOccupied()) { moves.Add(Square.GetOffsetedSquare(currSquare, 2, 0, board)); }
            if (!Square.GetOffsetedSquare(currSquare, -2, 0, board).IsOccupied()) { moves.Add(Square.GetOffsetedSquare(currSquare, -2, 0, board)); }
            return moves;
        }

        public override List<Square> RemoveBlockedSquares(List<Square> validMoves, Board.Board board)
        {
            throw new NotImplementedException();
        }
    }
}
