using MGChessLib.Common;
using MGChessLib.Squares;
using MGChessLib.Board;

namespace MGChessLib.Pieces 
{
    public abstract class Piece
    {
        protected string name;
        protected string color;
        protected Square currSquare;
        protected Uri imageSource;
        public Piece(string color)
        {
            this.color = color;
        }

        public string GetName() { return name; }
        public string GetColor() { return color; }
        public Square GetCurrSquare() { return currSquare; }
        public Uri GetImageSource() { return imageSource; }
        public void SetImageSource(Uri imageSource) { this.imageSource = imageSource; }
        public void SetCurrSquare(Square currSquare) { this.currSquare = currSquare; }

        public override string? ToString()
        {
            return "Piece: " + color + name + currSquare;
        }

        public abstract List<Square> GetValidMoves(Board.Board board);
        public abstract List<Square> RemoveBlockedSquares(List<Square> validMoves, Board.Board board);
    }
}
