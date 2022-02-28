using MGChessLib.Pieces;
using MGChessLib.Common;

namespace MGChessLib.Squares
{
    public class Square
    {
        private static readonly List<string> files = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H" };
        private static readonly List<string> ranks = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8" };

        private readonly string file;
        private readonly string rank;
        private readonly string name;
        public int Score = 0;

        private Piece currPiece;
        private readonly string squareColor; // color depends on location

        private bool isOccupied; // can get or set
        private bool isHit; // is the square hit by a piece

        // values are added during instantiation
        public Square(string file, string rank)
        {
            this.file = file;
            this.rank = rank;
            this.squareColor = ((files.IndexOf(this.file) + ranks.IndexOf(this.rank)) % 2 == 0) ? Color.Dark.ToString() : Color.Light.ToString();
            this.isOccupied = false;
            this.isHit = false;
            this.name = this.file + this.rank;
        }

        public Piece GetCurrPiece() { return this.currPiece; }
        public void SetCurrPiece(Piece piece) { this.currPiece = piece; }
        public bool IsOccupied() { return isOccupied; } // get
        public void SetOccupied(bool occupied) { isOccupied = occupied; } // set
        public string GetFile() { return file; }
        public string GetRank() { return rank; }
        public string GetName() { return name; }
        public string GetSquareColor() { return squareColor; }
        public static List<string> GetFiles() { return files; }
        public static List<string> GetRanks() { return ranks; }
        public int GetFileIndex(string file) { return files.IndexOf(file); }
        public int GetRankIndex(string rank) { return ranks.IndexOf(rank); }
        /// <summary>
        /// Returns the offset square of a given square in the given board, 
        /// all offsets that are outside the board are excluded.
        /// </summary>
        /// <param name="square"></param>
        /// <param name="fileOffset"></param>
        /// <param name="rankOffset"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public static Square GetOffsetedSquare(Square square, int fileOffset, int rankOffset, Board.Board board)
        {
            // if (the resultant square is in the board 8x8 squares
            if (Enumerable.Range(0, 8).Contains(square.GetRankIndex(square.GetRank()) + rankOffset) &
                Enumerable.Range(0, 8).Contains(square.GetFileIndex(square.GetFile()) + fileOffset))
            {
                int rankIndex = square.GetRankIndex(square.GetRank());
                int fileIndex = square.GetFileIndex(square.GetFile());

                return board.GetSquareByIndex(fileIndex + fileOffset, rankIndex + rankOffset);
            }
            return new Square("","");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="board">the game board</param>
        /// <param name="color">attacking pieces color</param>
        /// <returns>Whether the square is hit or not</returns>
        /// <out>hitIndex {0: not hit, 1: hit by light pieces, 2: hit by dark pieces, 3: hit by both}</out>
        public bool IsHit(Board.Board board, string color) 
        {
            List<Piece> pieces = board.GetPieces(color);
            List<Square> hitSquares = pieces.SelectMany(p => p.GetValidMoves(board)).ToList();

            if (hitSquares.Contains(this)) { isHit = true; return isHit; }
            else { isHit = false; return isHit; }
        }

        // should return attacking pieces

        // override the Equal and GetHashCode to be able to compare two locations to each other
        public override bool Equals(object? obj)
        {
            if (this == obj) return true;
            if (obj.GetType() != typeof(Square)) return false;
            
            var item = obj as Square;
            if (item == null) { return false; }
            return file.Equals(item.file) && rank.Equals(item.rank);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(file, rank);
        }

        public override string ToString()
        {
            return ((isOccupied) ? squareColor[0]+file+rank+currPiece.GetColor()[0] + "" + currPiece.GetName()[0]+currPiece.GetName()[1] : squareColor[0]+file+rank+"--");
        }
    }
}
