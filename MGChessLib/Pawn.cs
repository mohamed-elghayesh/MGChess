namespace MGChessLib
{
    public class Pawn : Piece
    {
        private bool firstMove = true;

        public bool hasMoved() { return !firstMove; }

        /// <summary>
        /// Captures the pieces forward to the right or left. Handled in Pawn.Capture(currentSquare)
        /// Pawn.hasTarget must be True
        /// </summary>
        /// <param name="currentSquare"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override List<string> Capture(string currentSquare)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentSquare"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override List<string> GetAttackers(string currentSquare)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// First move: one ore two squares forward, Handled in  Pawn.FirstMove(currentSquare)
        /// Other moves: one square forward, Handled here in GetLegalSquare(currentSquare)
       
        /// </summary>
        /// <param name="currentSquare">
        /// string currentSquare e.g: A1, H8, ...etc
        /// </param>
        /// <returns>
        /// List of all legal moves or squares or locations.
        /// </returns>
        public override List<string> GetLegalSquares(string currentSquare)
        {
            // a list to be returned with the legal square moves
            List<string> legalSquares = new List<string>();

            // using the board's lists, files and ranks
            int fileIndex = Board.Files().IndexOf(currentSquare[0] + "");
            int rankIndex = Board.Ranks().IndexOf(currentSquare[1] + "");
            if (firstMove)
            {
                // Add the legal pawn moves to the list
                legalSquares.Add(Board.Files()[fileIndex] + Board.Ranks()[rankIndex + 1]);
                legalSquares.Add(Board.Files()[fileIndex] + Board.Ranks()[rankIndex + 2]);
            }
            else { legalSquares.Add(Board.Files()[fileIndex] + Board.Ranks()[rankIndex + 1]); }

            return legalSquares;
        }


        public override List<string> GetTargets(List<string> legalSquares)
        {
            throw new NotImplementedException();
        }

        public override bool HasAttackers(string currentSquare)
        {
            throw new NotImplementedException();
        }

        public override bool HasTargets(List<string> legalSquares)
        {
            throw new NotImplementedException();
        }
        // Move(string currentSquare, List<string> legalSquares)
        public override int Move()
        {
            firstMove = false;
            return 1;
        }
    }
}
