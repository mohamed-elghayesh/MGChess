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

        public bool IsCastleMove(Movement move, Board.Board board)
        {
            Square source = move.GetSourceSquare();
            Square target = move.GetTargetSquare();
            Piece piece = board.GetSquare(source.GetName()).GetCurrPiece();

            // king's conditions
            bool pieceIsKing = (piece.GetType() == typeof(King)) ? true : false;
            if (!pieceIsKing) { return false; }
            King king = (King)piece;
            if (!king.IsFirstMove) { return false; }

            // target square conditions
            bool kingSideCastle;
            if (target.GetFile() == "C") { kingSideCastle = false; }
            else if (target.GetFile() == "G") { kingSideCastle = true; }
            else { return false; } // cancel if target file is neither C nor G

            // rook's conditions
            string rank = king.GetCurrSquare().GetRank();
            Square kingRookSq = board.GetSquare("H" + rank);
            Square queenRookSq = board.GetSquare("A" + rank);

            if (kingSideCastle)
            {
                if (!kingRookSq.IsOccupied()) { return false; } // cancel if rook's square is empty (needed for next condition)
                if (kingRookSq.GetCurrPiece().GetType() != typeof(Rook)) { return false; } // cancel if the square doesn't hold a rook (needed for next condition) 
                if (!((Rook)kingRookSq.GetCurrPiece()).IsFirstMove) { return false; } // cancel if the rook has moved before
                // both F and G files must be empty for king-side castling
                if (board.GetSquare("F" + rank).IsOccupied()) { return false; }
                if (board.GetSquare("G" + rank).IsOccupied()) { return false; }
            }
            else
            {
                if (!queenRookSq.IsOccupied()) { return false; }
                if (queenRookSq.GetCurrPiece().GetType() != typeof(Rook)) { return false; }
                if (!((Rook)queenRookSq.GetCurrPiece()).IsFirstMove) { return false; }
                // files B, C, and D must be empty for queen-side castle
                if (board.GetSquare("B" + rank).IsOccupied()) { return false; }
                if (board.GetSquare("C" + rank).IsOccupied()) { return false; }
                if (board.GetSquare("D" + rank).IsOccupied()) { return false; }
            }

            // now every castle condition is met
            return true;
        }

        public bool IsPromotion(Movement move, Board.Board board)
        {
            Square source = move.GetSourceSquare();
            Square target = move.GetTargetSquare();
            Piece piece = board.GetSquare(source.GetName()).GetCurrPiece();

            bool pieceIsPawn = (piece.GetType() == typeof(Pawn)) ? true : false;
            if (!pieceIsPawn) { return false; }
            Pawn pawn = (Pawn)piece;

            if (pawn.GetColor() == Color.Light.ToString()) { if (target.GetRank() != "8") { return false; } }
            if (pawn.GetColor() == Color.Dark.ToString()) { if (target.GetRank() != "1") { return false; } }

            return true;
        }

    }
}
