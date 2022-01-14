namespace MGChessLib
{
    public abstract class Piece //: IPiece
    {
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? CurrentSquare { get; set; }
        /// <summary>
        /// Used to get a list of strings that represent the squares or locations
        /// of the attacking pieces which add pressure to the currentSqure.
        /// </summary>
        /// <param name="currentSquare">
        /// string currentSquare e.g: A1, H8, ...etc
        /// </param>
        /// <returns>
        /// List of all attacking squares.
        /// </returns>
        public abstract List<string> GetAttackers(string currentSquare);
        /// <summary>
        /// Used to confirm if currentSquare has attacking squares or pieces.
        /// </summary>
        /// <param name="currentSquare">
        /// string currentSquare e.g: A1, H8, ...etc
        /// </param>
        /// <returns>
        /// True if it has at least one attacking square
        /// </returns>
        public abstract bool HasAttackers(string currentSquare);
        /// <summary>
        /// Used to confirm if currentSquare has target squares or pieces to attack.
        /// </summary>
        /// <param name="legalSquares">
        /// List legalSquares is used from GetLegalSquares(string currentSquare) e.g: List<string> {A1, H8, ...etc}
        /// </param>
        /// <returns>
        /// True if at least one target is present in the legalSquares list, otherwise False.
        /// </returns>
        public abstract bool HasTargets(List<string> legalSquares);
        /// <summary>
        /// Used to get a list of strings that represent the legal squares or locations for each piece type.
        /// </summary>
        /// <param name="currentSquare">
        /// string currentSquare e.g: A1, H8, ...etc
        /// </param>
        /// <returns>
        /// List of all legal moves or squares or locations.
        /// </returns>
        public abstract List<string> GetLegalSquares(string currentSquare);
        /// <summary>
        /// Used to get a list of strings that represent the squares or locations
        /// of the target or pressured pieces which are found in the list of legalSquares.
        /// </summary>
        /// <param name="legalSquares">
        /// List legalSquares is used from GetLegalSquares(string currentSquare) e.g: List<string> {A1, H8, ...etc}
        /// </param>
        /// <returns>
        /// List of all target squares.
        /// </returns>
        public abstract List<string> GetTargets(List<string> legalSquares);
        
        public abstract List<string> Capture(string currentSquare);

        //public abstract string Move(string currentSquare, List<string> legalSquares);
        public abstract int Move();

        public override string ToString()
        {
            return $"{this.GetType().Name} : {Color} {Name}";
            // e.g, Night : White Knight
        }
    }
}
