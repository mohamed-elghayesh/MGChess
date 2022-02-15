using MGChessLib.Pieces;
using MGChessLib.Squares;

namespace MGChessLib.Common
{
    public class Movement
    {
        private static int counter = 0;
        private Square sourceSquare;
        private Square targetSquare;
        private Piece sourcePiece;
        private Piece targetPiece;

        //both squares refer to board squares
        public Movement(Square source, Square target, Board.Board board)
        {
            counter++;
            sourceSquare = board.GetSquare(source.GetName());
            targetSquare = board.GetSquare(target.GetName());
            if (sourceSquare.IsOccupied()) { sourcePiece = sourceSquare.GetCurrPiece(); };
            if (targetSquare.IsOccupied()) { targetPiece = targetSquare.GetCurrPiece(); };
        }

        public Square GetSourceSquare() { return sourceSquare; }
        public void SetSourceSquare(Square sourceSq) { sourceSquare = sourceSq; }

        public Square GetTargetSquare() { return targetSquare; }
        public void SetTargetSquare(Square targetSq) { targetSquare = targetSq; }

        public Piece GetPieceToMove() { return sourcePiece; }

        // validate that source square piece moves contains the target square
        public bool IsValidMove(Board.Board board, out string message)
        {
            if (!sourceSquare.IsOccupied()) { Reset(); message = "Empty square. Please select a piece to move."; return false; }
            if (targetSquare.IsOccupied()) { if (sourcePiece.GetColor() == targetPiece.GetColor()) { Reset(); message = "Select a piece to move."; return false; } }
            if (sourceSquare == targetSquare) { Reset(); message = "Can not move to the same square."; return false; }
            if (counter % 2 == 1 & sourcePiece.GetColor() == Color.Dark.ToString()) { Reset(); message = "Light colored pieces turn."; return false; }
            if (counter % 2 == 0 & sourcePiece.GetColor() == Color.Light.ToString()) { Reset(); message = "Dark colored pieces turn."; return false; }
            if (!sourcePiece.GetValidMoves(board).Contains(targetSquare)) { Reset(); message = $"{targetSquare.GetName()} is not a valid move for a {sourcePiece.GetName()} on {sourceSquare}."; return false; }

            message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
            return true;
        }

        private void Reset()
        {
            counter--;
        }
    }
}
