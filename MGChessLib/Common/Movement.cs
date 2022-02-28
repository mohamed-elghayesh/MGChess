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
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Source square piece</returns>
        public Piece GetPiece() { return sourcePiece; }

        // validate that source square piece moves contains the target square
        public bool IsValidMove(Board.Board board, out string message)
        {
            if (!sourceSquare.IsOccupied()) { CounterReset(1); message = "Empty square. Please select a piece to move."; return false; }
            if (targetSquare.IsOccupied()) { if (sourcePiece.GetColor() == targetPiece.GetColor()) { CounterReset(1); message = "Select a piece to move."; return false; } }
            if (sourceSquare == targetSquare) { CounterReset(1); message = "Can not move to the same square."; return false; }
            if (counter % 2 == 1 & sourcePiece.GetColor() == Color.Dark.ToString()) { CounterReset(1); message = "Light colored pieces turn."; return false; }
            if (counter % 2 == 0 & sourcePiece.GetColor() == Color.Light.ToString()) { CounterReset(1); message = "Dark colored pieces turn."; return false; }
            if (!sourcePiece.GetValidMoves(board).Contains(targetSquare)) { CounterReset(1); message = $"{targetSquare.GetName()} is not a valid move for a {sourcePiece.GetName()} on {sourceSquare}."; return false; }

            message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
            return true;
        }

        public void CounterReset(int reverse)
        {
            counter -= reverse;
        }

        public bool IsCastleMove(Board.Board board, out string message)
        {
            //Square source = move.GetSourceSquare();
            //Square target = move.GetTargetSquare();
            Piece piece = board.GetSquare(sourceSquare.GetName()).GetCurrPiece();
            if (sourceSquare == targetSquare) { CounterReset(1); message = "Can not move to the same square."; return false; }

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
            if (targetSquare.GetFile() == "C") { kingSideCastle = false; }
            else if (targetSquare.GetFile() == "G") { kingSideCastle = true; }
            else 
            {
                message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
                return false; 
            } // cancel if target file is neither C nor G

            // rook's conditions
            string rank = king.GetCurrSquare().GetRank();
            Square kingRookSq = board.GetSquare("H" + rank);
            Square queenRookSq = board.GetSquare("A" + rank);
            string attackerColor = (king.GetColor() == MGChessLib.Common.Color.Light.ToString()) ?
                                   MGChessLib.Common.Color.Dark.ToString() : MGChessLib.Common.Color.Light.ToString();

            if (kingSideCastle)
            {
                // both F and G files must be empty for king-side castling
                if (board.GetSquare("F" + rank).IsOccupied()) { CounterReset(1); message = "Cannot castle, there is a piece occupying the F file."; return false; }
                if (board.GetSquare("F" + rank).IsHit(board, attackerColor)) { CounterReset(1); message = "Cannot castle, F file is under attack"; return false; }
                if (board.GetSquare("G" + rank).IsOccupied()) { CounterReset(1); message = "Cannot castle, there is a piece occupying the G file."; return false; }
                if (board.GetSquare("G" + rank).IsHit(board, attackerColor)) { CounterReset(1); message = "Cannot castle, G file is under attack"; return false; }

                if (!kingRookSq.IsOccupied()) { CounterReset(1); message = "Cannot castle, rook's square is empty!"; return false; } // cancel if rook's square is empty (needed for next condition)
                if (kingRookSq.GetCurrPiece().GetType() != typeof(Rook)) { CounterReset(1); message = "Cannot castle, the piece on target square is not a rook!"; return false; } // cancel if the square doesn't hold a rook (needed for next condition) 
                if (!((Rook)kingRookSq.GetCurrPiece()).IsFirstMove) { CounterReset(1); message = "Cannot castle, the rook has moved!"; return false; } // cancel if the rook has moved before
            }
            else
            {
                // files B, C, and D must be empty for queen-side castle
                if (board.GetSquare("B" + rank).IsOccupied()) { CounterReset(1); message = "Cannot castle, there is a piece occupying the B file."; return false; }
                if (board.GetSquare("B" + rank).IsHit(board, attackerColor)) { CounterReset(1); message = "Cannot castle, B file is under attack"; return false; }
                if (board.GetSquare("C" + rank).IsOccupied()) { CounterReset(1); message = "Cannot castle, there is a piece occupying the C file."; return false; }
                if (board.GetSquare("C" + rank).IsHit(board, attackerColor)) { CounterReset(1); message = "Cannot castle, C file is under attack"; return false; }
                if (board.GetSquare("D" + rank).IsOccupied()) { CounterReset(1); message = "Cannot castle, there is a piece occupying the D file."; return false; }
                if (board.GetSquare("D" + rank).IsHit(board, attackerColor)) { CounterReset(1); message = "Cannot castle, D file is under attack"; return false; }

                if (!queenRookSq.IsOccupied()) { CounterReset(1); message = "Cannot castle, rook's square is empty!"; return false; }
                if (queenRookSq.GetCurrPiece().GetType() != typeof(Rook)) { CounterReset(1); message = "Cannot castle, the piece on target square is not a rook!"; return false; }
                if (!((Rook)queenRookSq.GetCurrPiece()).IsFirstMove) { CounterReset(1); message = "Cannot castle, the rook has moved!"; return false; }

            }
            // now every castle condition is met

            message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} castled to {targetSquare.GetName()}";
            return true;
        }

        public bool IsPromotion(Board.Board board, out string message)
        {
            Piece piece = board.GetSquare(sourceSquare.GetName()).GetCurrPiece();

            bool pieceIsPawn = (piece.GetType() == typeof(Pawn)) ? true : false;
            if (!pieceIsPawn) 
            { 
                message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
                return false; 
            }
            
            Pawn pawn = (Pawn)piece;
            if (pawn.GetColor() == Color.Light.ToString()) 
            { 
                if (targetSquare.GetRank() != "8") 
                { 
                    message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
                    return false; 
                } 
            }
            if (pawn.GetColor() == Color.Dark.ToString()) 
            { 
                if (targetSquare.GetRank() != "1") 
                { 
                    message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} moved to {targetSquare.GetName()}";
                    return false; 
                } 
            }

            message = $"{counter}: {sourceSquare.GetName()} {sourcePiece.GetColor()} colored {sourcePiece.GetName()} has promoted to ";
            return true;
        }
        // for this to work, the total fire power of a squad should be added to a list or a dictionary (square, int that reflects score)
        // score: how many attackers and how many defenders score (10, 30, 50, 90, 900) score model
        // if (the opposite king was occupying a square under direct fire, it must move, or be shielded by another piece, or game ends
        // square score should be a property in a square.
        
        // at the end of each move check for checks, so move() then see if a check happens
        // after the move , the piece will be on the target square
        public bool IsCheck(Board.Board board, out string message)
        {
            List<Square> squares = sourcePiece.GetValidMoves(board);
            squares.RemoveAll(sq => sq.IsOccupied() == false);
            Square? checkSq = squares.Find(sq => sq.GetCurrPiece().GetType() == typeof(King) && (sq.GetCurrPiece().GetColor() != sourcePiece.GetColor()));

            if(checkSq != null) 
            { 
                ((King)checkSq.GetCurrPiece()).IsChecked = true;
                message = ": CHECK!!";
                return true; 
            }
            message = ": Not a check :)";
            return false;
        }
    }
}
