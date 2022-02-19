using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MGChessLib.Common;
using MGChessLib.Pieces;
using MGChessLib.Squares;
using MGChessLib.Board;
using static ChessWPF.Helper;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace ChessWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        bool whiteTurn = true;
        SolidColorBrush lightSqColor = Brushes.BurlyWood;
        SolidColorBrush darkSqColor = Brushes.DarkGoldenrod;

        Board board = new Board();
        List<Label> moveSplits = new List<Label>(2);

        readonly Rook wR1 = new Rook(MGChessLib.Common.Color.Light.ToString());
        readonly Knight wN1 = new Knight(MGChessLib.Common.Color.Light.ToString());
        readonly Bishop wB1 = new Bishop(MGChessLib.Common.Color.Light.ToString());
        readonly Queen wQ = new Queen(MGChessLib.Common.Color.Light.ToString());
        readonly King wK = new King(MGChessLib.Common.Color.Light.ToString());
        readonly Rook wR2 = new Rook(MGChessLib.Common.Color.Light.ToString());
        readonly Knight wN2 = new Knight(MGChessLib.Common.Color.Light.ToString());
        readonly Bishop wB2 = new Bishop(MGChessLib.Common.Color.Light.ToString());
        readonly Pawn wP1 = new Pawn(MGChessLib.Common.Color.Light.ToString());
        readonly Pawn wP2 = new Pawn(MGChessLib.Common.Color.Light.ToString());
        readonly Pawn wP3 = new Pawn(MGChessLib.Common.Color.Light.ToString());
        readonly Pawn wP4 = new Pawn(MGChessLib.Common.Color.Light.ToString());
        readonly Pawn wP5 = new Pawn(MGChessLib.Common.Color.Light.ToString());
        readonly Pawn wP6 = new Pawn(MGChessLib.Common.Color.Light.ToString());
        readonly Pawn wP7 = new Pawn(MGChessLib.Common.Color.Light.ToString());
        readonly Pawn wP8 = new Pawn(MGChessLib.Common.Color.Light.ToString());
        readonly Rook bR1 = new Rook(MGChessLib.Common.Color.Dark.ToString());
        readonly Knight bN1 = new Knight(MGChessLib.Common.Color.Dark.ToString());
        readonly Bishop bB1 = new Bishop(MGChessLib.Common.Color.Dark.ToString());
        readonly Queen bQ = new Queen(MGChessLib.Common.Color.Dark.ToString());
        readonly King bK = new King(MGChessLib.Common.Color.Dark.ToString());
        readonly Rook bR2 = new Rook(MGChessLib.Common.Color.Dark.ToString());
        readonly Knight bN2 = new Knight(MGChessLib.Common.Color.Dark.ToString());
        readonly Bishop bB2 = new Bishop(MGChessLib.Common.Color.Dark.ToString());
        readonly Pawn bP1 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
        readonly Pawn bP2 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
        readonly Pawn bP3 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
        readonly Pawn bP4 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
        readonly Pawn bP5 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
        readonly Pawn bP6 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
        readonly Pawn bP7 = new Pawn(MGChessLib.Common.Color.Dark.ToString());
        readonly Pawn bP8 = new Pawn(MGChessLib.Common.Color.Dark.ToString());

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayBoardPieces();
        }

        private void DisplayBoardPieces()
        {
            foreach (Label square in ChessGrid.Children.OfType<Label>()) { square.Content = null; }
            board.LoadPieces();
            BoardToGUI();
        }

        // after load and after movement call this to show on GUI
        private void BoardToGUI()
        {
            foreach (List<Square> rank in board.ChessBoard)
            {
                foreach (Square square in rank)
                {
                    Label currLabel = ChessGrid.Children.OfType<Label>().First(lbl => lbl.Name == square.GetName());
                    if (square.IsOccupied())
                    {
                        Piece piece = square.GetCurrPiece();
                        Image img = new Image() { Source = new BitmapImage(piece.GetImageSource()) };
                        currLabel.Content = img;
                    }
                }
            }
        }

        private void SquareMouseClick(object sender, MouseButtonEventArgs e)
        {
            RemoveHighlight();
            Label label = (Label)sender;
            Square square = board.GetSquare(label.Name);
            
            if (moveSplits.Count == 0) { moveSplits.Add(label); HighLight(square); }
            else if (moveSplits.Count == 1) { moveSplits.Add(label); MakeMove(moveSplits, board); moveSplits.Clear(); }
            
        }

        private void MakeMove(List<Label> moveList, Board board)
        {
            Square source = board.GetSquare(moveList[0].Name);
            Square target = board.GetSquare(moveList[1].Name);
            string message = "";
            Movement move = new Movement(source, target, board);

            if (move.IsValidMove(board, out message))
            {
                if (move.IsCastleMove(move, board)) // initiate castling sequence
                {
                    // remove the label's content of the designated rook
                    string rank = source.GetRank();
                    if (target.GetFile() == "G") { ChessGrid.Children.OfType<Label>().First(x => x.Name == "H" + rank).Content = null; }
                    else if(target.GetFile() == "C") { ChessGrid.Children.OfType<Label>().First(x => x.Name == "A" + rank).Content = null; }
                }

                else if (move.IsPromotion(move, board)) // initiate promotion sequence
                {
                    Pawn promotionPawn = (Pawn)move.GetPieceToMove();

                    if (promotionPawn.GetColor() == MGChessLib.Common.Color.Light.ToString())
                    {
                        Popup lightPopup = LightPopup;
                        lightPopup.AllowsTransparency = true;
                        StackPanel panel = LightPanel;
                        panel.Background = lightSqColor;
                        
                        Image queenImage = new Image() { Source = new BitmapImage(wQ.GetImageSource()), Name = "queen" };
                        Image rookImage = new Image() { Source = new BitmapImage(wR1.GetImageSource()), Name = "rook" };
                        Image bishopImage = new Image() { Source = new BitmapImage(wB1.GetImageSource()), Name = "bishop" };
                        Image knightImage = new Image() { Source = new BitmapImage(wN1.GetImageSource()), Name = "knight" };

                        panel.MouseDown += (s, e) => StackPanelMouseDown(s, e, move); // add the instance to event handler

                        panel.Children.Add(queenImage);
                        panel.Children.Add(rookImage);
                        panel.Children.Add(bishopImage);
                        panel.Children.Add(knightImage);

                        lightPopup.Child = panel;
                        lightPopup.Width = 150;
                        lightPopup.Height = 40;
                        lightPopup.PlacementTarget = ChessGrid.Children.OfType<Label>().First(x => x.Name == move.GetTargetSquare().GetName());
                        //lightPopup.PlacementRectangle = new Rect(0, 0, 120, 30);
                        //lightPopup.AllowsTransparency = true;
                        //lightPopup.PopupAnimation = PopupAnimation.Fade;
                        lightPopup.IsOpen = true;

                    }
                    else if (promotionPawn.GetColor() == MGChessLib.Common.Color.Dark.ToString())
                    {
                        Popup darkPopup = DarkPopup;
                        darkPopup.AllowsTransparency = true;
                        StackPanel panel = DarkPanel;
                        panel.Background = lightSqColor;

                        Image queenImage = new Image() { Source = new BitmapImage(bQ.GetImageSource()), Name = "queen" };
                        Image rookImage = new Image() { Source = new BitmapImage(bR1.GetImageSource()), Name = "rook" };
                        Image bishopImage = new Image() { Source = new BitmapImage(bB1.GetImageSource()), Name = "bishop" };
                        Image knightImage = new Image() { Source = new BitmapImage(bN1.GetImageSource()), Name = "knight" };

                        panel.MouseDown += (s, e) => StackPanelMouseDown(s, e, move); // overload event handler

                        panel.Children.Add(queenImage);
                        panel.Children.Add(rookImage);
                        panel.Children.Add(bishopImage);
                        panel.Children.Add(knightImage);

                        darkPopup.Child = panel;
                        darkPopup.Width = 150;
                        darkPopup.Height = 40;
                        darkPopup.PlacementTarget = ChessGrid.Children.OfType<Label>().First(x => x.Name == move.GetTargetSquare().GetName());

                        darkPopup.IsOpen = true;
                    }
                }
                else // just move
                {
                    board.SetMove(move, null);
                }
                moveList[0].Content = null;
                lblStatus.Text = message;
                BoardToGUI();
            }
            else { lblStatus.Text = message; }
        }

        

        private void RemoveHighlight()
        {
            List<string> files = Square.GetFiles();
            List<string> ranks = Square.GetRanks();

            foreach (Label lbl in ChessGrid.Children.OfType<Label>())
            {
                if (lbl.Content != null)
                {
                    if (lbl.Content.GetType() == typeof(Ellipse)) { lbl.Content = null; }
                }
                if (lbl.Background == Brushes.Goldenrod)
                {
                    lbl.Background = ((files.IndexOf(GetFile(lbl)) + ranks.IndexOf(GetRank(lbl))) % 2 == 0) ? darkSqColor : lightSqColor;
                }
            }
        }

        public void HighLight(Square sourceSq)
        {
            if (sourceSq.IsOccupied())
            {
                Piece piece = sourceSq.GetCurrPiece();
                List<Square> validMoves = piece.GetValidMoves(board);

                foreach (Square square in validMoves)
                {
                    Label lbl = ChessGrid.Children.OfType<Label>().First(x => x.Name == square.GetName());

                    if (!square.IsOccupied())
                    {
                        SolidColorBrush fillColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(50, 50, 50, 50));
                        Ellipse ellipse = new Ellipse();
                        ellipse.Fill = fillColor;
                        ellipse.Width = 10;
                        ellipse.Height = 10;

                        lbl.Content = ellipse;
                    }
                    else 
                    { 
                        lbl.Background = Brushes.Goldenrod; 
                    }
                }
            }
        }

        private void StackPanelMouseDown(object sender, MouseButtonEventArgs e)
        {
            lblStatus.Text = ((Image)e.OriginalSource).Name;
        }

        private void StackPanelMouseDown(object s, MouseButtonEventArgs e, Movement move)
        {
            Image image = (Image)e.OriginalSource;
            string color = move.GetPieceToMove().GetColor();
            Piece piece = (image.Name == "queen") ? new Queen(color) : (image.Name == "rook") ? new Rook(color) : 
                          (image.Name == "bishop") ? new Bishop(color) : new Knight(color);

            board.Promote(move, piece);

            DarkPopup.IsOpen = false;
            LightPopup.IsOpen = false;

            BoardToGUI();
        }

        private void btnUndoClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
