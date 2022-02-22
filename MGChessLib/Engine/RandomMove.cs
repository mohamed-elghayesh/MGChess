using MGChessLib.Common;
using MGChessLib.Pieces;
using MGChessLib.Squares;

namespace MGChessLib.Engine
{
    public class RandomMove
    {
        public string pieceColor { get; set; }

        // pieces and turns are set from application.
        public RandomMove(string pieceColor)
        {
            this.pieceColor = pieceColor; // color determines strategy, but not for random moves
        }

        public Movement GetRandomMove(Board.Board board)
        {
            Random rand = new Random();
            Square source = new Square("", "");
            Square target = new Square("", "");
            List<Square> colorOccupiedSquares = GetSquaresByPieceColor(this.pieceColor, board);

            foreach (Square occupiedSq in colorOccupiedSquares)
            {
                source = colorOccupiedSquares[rand.Next(colorOccupiedSquares.Count)];
                Piece sourcePiece = board.GetSquare(source.GetName()).GetCurrPiece();
                List<Square> validMoves = sourcePiece.GetValidMoves(board);

                if (validMoves.Count != 0)
                {
                    target = validMoves[rand.Next(validMoves.Count)];
                    break;
                }
            }
            return new Movement(source, target, board); // application will receive move and apply it
        }

        private List<Square> GetSquaresByPieceColor(string pieceColor, Board.Board board)
        {
            List<Square> result = new List<Square>();

            foreach (List<Square> row in board.ChessBoard)
            {
                foreach (Square square in row)
                {
                    if(square.GetCurrPiece().GetColor() == pieceColor) { result.Add(square); }
                }
            }
            return result;
        }
    }
}
