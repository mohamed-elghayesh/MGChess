using MGChessLib.Squares;
using MGChessLib.Common;
using MGChessLib.Pieces;

namespace MGChessLib.Board
{
    public class Board
    {
        private readonly List<List<Square>> chessBoard = new List<List<Square>>(8);
        private readonly List<Square> boardRank;

        public Board()
        {
            // loop to fill the board with squares
            // i represents the rank increment
            for (int i = 0; i < chessBoard.Capacity; i++)
            {
                boardRank = new List<Square>(8);
                for (int j = 0; j < boardRank.Capacity; j++ )
                {
                    Square currSquare = new Square(Square.GetFiles()[j], Square.GetRanks()[i]);
                    boardRank.Add(currSquare);
                }
                chessBoard.Add(boardRank);
                boardRank = new List<Square>(8);
            }
        }

        public List<Square> GetBoardRank(int rankIndex)
        {
            return chessBoard[rankIndex];
        }

        public Square GetSquare(string file, string rank)
        {
            int fileIndex = Square.GetFiles().IndexOf(file);
            int rankIndex = Square.GetRanks().IndexOf(rank);
            return chessBoard[rankIndex][fileIndex];
        }

        public Square GetSquare(string name) { return GetSquare($"{name[0]}", $"{name[1]}"); }

        public Square GetSquareByIndex(int fileIndex, int rankIndex)
        {
            return chessBoard[rankIndex][fileIndex];
        }

        public List<List<Square>> ChessBoard => chessBoard;

        public Board LoadPieces()
        {
            #region piece declaration
            Rook wR1 = new Rook(MGChessLib.Common.Color.Light.ToString());
            Knight wN1 = new Knight(MGChessLib.Common.Color.Light.ToString());
            Bishop wB1 = new Bishop(MGChessLib.Common.Color.Light.ToString());
            Queen wQ = new Queen(MGChessLib.Common.Color.Light.ToString());
            King wK = new King(MGChessLib.Common.Color.Light.ToString());
            Rook wR2 = new Rook(MGChessLib.Common.Color.Light.ToString());
            Knight wN2 = new Knight(MGChessLib.Common.Color.Light.ToString());
            Bishop wB2 = new Bishop(MGChessLib.Common.Color.Light.ToString());
            Pawn wP1 = new Pawn(MGChessLib.Common.Color.Light.ToString());
            Pawn wP2 = new Pawn(MGChessLib.Common.Color.Light.ToString());
            Pawn wP3 = new Pawn(MGChessLib.Common.Color.Light.ToString());
            Pawn wP4 = new Pawn(MGChessLib.Common.Color.Light.ToString());
            Pawn wP5 = new Pawn(MGChessLib.Common.Color.Light.ToString());
            Pawn wP6 = new Pawn(MGChessLib.Common.Color.Light.ToString());
            Pawn wP7 = new Pawn(MGChessLib.Common.Color.Light.ToString());
            Pawn wP8 = new Pawn(MGChessLib.Common.Color.Light.ToString());
            Rook bR1 = new Rook(MGChessLib.Common.Color.Dark.ToString());
            Knight bN1 = new Knight(MGChessLib.Common.Color.Dark.ToString());
            Bishop bB1 = new Bishop(MGChessLib.Common.Color.Dark.ToString());
            Queen bQ = new Queen(MGChessLib.Common.Color.Dark.ToString());
            King bK = new King(MGChessLib.Common.Color.Dark.ToString());
            Rook bR2 = new Rook(MGChessLib.Common.Color.Dark.ToString());
            Knight bN2 = new Knight(MGChessLib.Common.Color.Dark.ToString());
            Bishop bB2 = new Bishop(MGChessLib.Common.Color.Dark.ToString());
            Pawn bP1 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
            Pawn bP2 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
            Pawn bP3 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
            Pawn bP4 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
            Pawn bP5 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
            Pawn bP6 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
            Pawn bP7 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
            Pawn bP8 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
            #endregion

            List<Square> squares = new List<Square>(32) { GetSquare("A1"), GetSquare("B1"), GetSquare("C1"), GetSquare("D1"), GetSquare("E1"), GetSquare("F1"), GetSquare("G1"), GetSquare("H1"),
                                                          GetSquare("A2"), GetSquare("B2"), GetSquare("C2"), GetSquare("D2"), GetSquare("E2"), GetSquare("F2"), GetSquare("G2"), GetSquare("H2"),
                                                          GetSquare("A8"), GetSquare("B8"), GetSquare("C8"), GetSquare("D8"), GetSquare("E8"), GetSquare("F8"), GetSquare("G8"), GetSquare("H8"),
                                                          GetSquare("A7"), GetSquare("B7"), GetSquare("C7"), GetSquare("D7"), GetSquare("E7"), GetSquare("F7"), GetSquare("G7"), GetSquare("H7") };
            
            List<Piece> pieces = new List<Piece>(32) { wR1, wN1, wB1, wQ, wK, wB2, wN2, wR2, wP1, wP2, wP3, wP4, wP5, wP6, wP7, wP8,
                                                       bR1, bN1, bB1, bQ, bK, bB2, bN2, bR2, bP1, bP2, bP3, bP4, bP5, bP6, bP7, bP8 };

            for (int i = 0; i < squares.Count; i++)
            {
                pieces[i].SetCurrSquare(squares[i]);
                squares[i].SetCurrPiece(pieces[i]);
                squares[i].SetOccupied(true);
            }
            return this;
        }

        // set castling and promotions
        public Board SetMove(Movement move, Piece promotionPiece)
        {
            Square source = move.GetSourceSquare();
            Square target = move.GetTargetSquare();
            Piece piece = this.GetSquare(source.GetName()).GetCurrPiece();
            string pieceRank = piece.GetCurrSquare().GetRank();

            if (move.IsCastleMove(move, this)) 
            {
                // king's move
                this.GetSquare(source.GetName()).SetOccupied(false);
                this.GetSquare(source.GetName()).SetCurrPiece(null);
                this.GetSquare(target.GetName()).SetOccupied(true);
                this.GetSquare(target.GetName()).SetCurrPiece(piece);
                piece.SetCurrSquare(target);
                // rook's move
                if (piece.GetCurrSquare().GetFile() == "G")
                {
                    Rook rook = (Rook)GetSquare("H" + pieceRank).GetCurrPiece();
                    GetSquare("F" + pieceRank).SetCurrPiece(rook);
                    GetSquare("F" + pieceRank).SetOccupied(true);
                    GetSquare("H" + pieceRank).SetCurrPiece(null);
                    GetSquare("H" + pieceRank).SetOccupied(false);
                    rook.SetCurrSquare(GetSquare("F" + pieceRank));
                }

                if (piece.GetCurrSquare().GetFile() == "C")
                {
                    Rook rook = (Rook)GetSquare("A" + pieceRank).GetCurrPiece();
                    GetSquare("D" + pieceRank).SetCurrPiece(rook);
                    GetSquare("D" + pieceRank).SetOccupied(true);
                    GetSquare("A" + pieceRank).SetCurrPiece(null);
                    GetSquare("A" + pieceRank).SetOccupied(false);
                    rook.SetCurrSquare(GetSquare("D" + pieceRank));
                }

                return this; 
            }

            // source piece is rook: falsify rook's first move
            if ((piece.GetType() == typeof(Rook)) && ((Rook)piece).IsFirstMove) { ((Rook)piece).IsFirstMove = false; }
            // source piece is king: falsify king's first move
            if ((piece.GetType() == typeof(King)) && ((King)piece).IsFirstMove) { ((King)piece).IsFirstMove = false; }
            // source piece is pawn: falsify pawn's first move
            if ((piece.GetType() == typeof(Pawn)) && ((Pawn)piece).IsFirstMove) { ((Pawn)piece).IsFirstMove = false; }

            this.GetSquare(source.GetName()).SetOccupied(false);
            this.GetSquare(source.GetName()).SetCurrPiece(null);
            this.GetSquare(target.GetName()).SetOccupied(true);
            this.GetSquare(target.GetName()).SetCurrPiece(piece);
            piece.SetCurrSquare(target);

            return this;
        }

        public void Promote(Movement move, Piece promotionPiece)
        {
            Square source = move.GetSourceSquare();
            Square target = move.GetTargetSquare();

            this.GetSquare(source.GetName()).SetOccupied(false);
            this.GetSquare(source.GetName()).SetCurrPiece(null);
            this.GetSquare(target.GetName()).SetCurrPiece((Piece)promotionPiece);
            this.GetSquare(target.GetName()).SetOccupied(true);
            promotionPiece.SetCurrSquare(target);
        }
        
        public void PrintBoard()
        {
            int counter = 0;
            chessBoard.Reverse();
            foreach (List<Square> rank in chessBoard)
            {
                Console.Write(rank[0].GetRankIndex(rank[0].GetRank()) + 1 + "\t");

                foreach (Square square in rank)
                {
                    Console.Write(square + "\t");
                    counter++;
                }
                Console.WriteLine();
            }
            Console.WriteLine("\tA\tB\tC\tD\tE\tF\tG\tH");
            Console.WriteLine("\n Square Count: " + counter);
        }
    }
}
