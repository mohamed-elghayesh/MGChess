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

        public Piece GetPiece() { return sourcePiece; }

        // validate that source square piece moves contains the target square
        public bool IsValidMove(Board.Board board, out string message)
        {
            if (!sourceSquare.IsOccupied()) { CounterReset(); message = "Empty square. Please select a piece to move."; return false; }
            if (targetSquare.IsOccupied()) { if (sourcePiece.GetColor() == targetPiece.GetColor()) { CounterReset(); message = "Select a piece to move."; return false; } }
            if (sourceSquare == targetSquare) { CounterReset(); message = "Can not move to the same square."; return false; }
            if (counter % 2 == 1 & sourcePiece.GetColor() == Color.Dark.ToString()) { CounterReset(); message = "Light colored pieces turn."; return false; }
            if (counter % 2 == 0 & sourcePiece.GetColor() == Color.Light.ToString()) { CounterReset(); message = "Dark colored pieces turn."; return false; }
            if (!sourcePiece.GetValidMoves(board).Contains(targetSquare)) { CounterReset(); message = $"{targetSquare.GetName()} is not a valid move for a {sourcePiece.GetName()} on {sourceSquare}."; return false; }

            message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
            return true;
        }

        public void CounterReset()
        {
            counter--;
        }

        public bool IsCastleMove(Movement move, Board.Board board, out string message)
        {
            Square source = move.GetSourceSquare();
            Square target = move.GetTargetSquare();
            Piece piece = board.GetSquare(source.GetName()).GetCurrPiece();

            // king's conditions
            bool pieceIsKing = (piece.GetType() == typeof(King)) ? true : false;
            if (!pieceIsKing) 
            { 
                message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
                return false; 
            }
            King king = (King)piece;
            if (!king.IsFirstMove) { message = "The King has already moves"; return false; }

            // target square conditions
            bool kingSideCastle;
            if (target.GetFile() == "C") { kingSideCastle = false; }
            else if (target.GetFile() == "G") { kingSideCastle = true; }
            else { message = "The king cannot castle in this square."; return false; } // cancel if target file is neither C nor G

            // rook's conditions
            string rank = king.GetCurrSquare().GetRank();
            Square kingRookSq = board.GetSquare("H" + rank);
            Square queenRookSq = board.GetSquare("A" + rank);

            if (kingSideCastle)
            {
                if (!kingRookSq.IsOccupied()) { message = "Cannot castle, rook's square is empty!"; return false; } // cancel if rook's square is empty (needed for next condition)
                if (kingRookSq.GetCurrPiece().GetType() != typeof(Rook)) { message = "Cannot castle, the piece on target square is not a rook!"; return false; } // cancel if the square doesn't hold a rook (needed for next condition) 
                if (!((Rook)kingRookSq.GetCurrPiece()).IsFirstMove) { message = "Cannot castle, the rook has moved!"; return false; } // cancel if the rook has moved before
                // both F and G files must be empty for king-side castling
                if (board.GetSquare("F" + rank).IsOccupied()) { message = "Cannot castle, there is a piece occupying the F file."; return false; }
                if (board.GetSquare("G" + rank).IsOccupied()) { message = "Cannot castle, there is a piece occupying the G file."; return false; }
                // both F and G must not be hit by any of opponent's pieces
                // cannot move the {} square.IsHit()
            }
            else
            {
                if (!queenRookSq.IsOccupied()) { message = "Cannot castle, rook's square is empty!"; return false; }
                if (queenRookSq.GetCurrPiece().GetType() != typeof(Rook)) { message = "Cannot castle, the piece on target square is not a rook!"; return false; }
                if (!((Rook)queenRookSq.GetCurrPiece()).IsFirstMove) { message = "Cannot castle, the rook has moved!"; return false; }
                // files B, C, and D must be empty for queen-side castle
                if (board.GetSquare("B" + rank).IsOccupied()) { message = "Cannot castle, there is a piece occupying the B file."; return false; }
                if (board.GetSquare("C" + rank).IsOccupied()) { message = "Cannot castle, there is a piece occupying the C file."; return false; }
                if (board.GetSquare("D" + rank).IsOccupied()) { message = "Cannot castle, there is a piece occupying the D file."; return false; }
            }
            // now every castle condition is met
            message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} castled to {targetSquare.GetName()}";
            return true;
        }

        public bool IsPromotion(Movement move, Board.Board board, out string message)
        {
            Square source = move.GetSourceSquare();
            Square target = move.GetTargetSquare();
            Piece piece = board.GetSquare(source.GetName()).GetCurrPiece();

            bool pieceIsPawn = (piece.GetType() == typeof(Pawn)) ? true : false;
            if (!pieceIsPawn) 
            { 
                message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
                return false; 
            }
            
            Pawn pawn = (Pawn)piece;
            if (pawn.GetColor() == Color.Light.ToString()) 
            { 
                if (target.GetRank() != "8") 
                { 
                    message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
                    return false; 
                } 
            }
            if (pawn.GetColor() == Color.Dark.ToString()) 
            { 
                if (target.GetRank() != "1") 
                { 
                    message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
                    return false; 
                } 
            }

            message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} promoted to a {targetSquare.GetCurrPiece()}";
            return true;
        }
        // for this to work, the total fire power of a squad should be added to a list or a dictionary (square, int that reflects score)
        // score: how many attackers and how many defenders score (10, 30, 50, 90, 900) score model
        // if (the opposite king was occupying a square under direct fire, it must move, or be shielded by another piece, or game ends
        // square score should be a property in a square.
        public bool IsCheck(Movement move, Board.Board board)
        {
            Piece piece = move.GetPiece();
            List<Square> squares = piece.GetValidMoves(board);
            Square? checkSq = squares.Find(sq => (sq.GetCurrPiece().GetType() == typeof(King) && (sq.GetCurrPiece().GetColor() != piece.GetColor()) ));

            if(checkSq != null) { ((King)checkSq.GetCurrPiece()).IsUnderCheck = true; return true; }
            return false;
        }
    }
}
