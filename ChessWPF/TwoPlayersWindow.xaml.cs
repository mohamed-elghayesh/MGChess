using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

using MGChessLib.Common;
using MGChessLib.Pieces;
using MGChessLib.Squares;
using MGChessLib.Board;
using static ChessWPF.Helper;

namespace ChessWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TwoPlayersWindow : Window
    {
        #region Variables
        SolidColorBrush lightSqColor = Brushes.BurlyWood;
        SolidColorBrush darkSqColor = Brushes.DarkGoldenrod;

        Board board = new Board();
        List<Label> moveSplits = new List<Label>(2);
        #endregion

        public TwoPlayersWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayBoardPieces();
        }

        private void DisplayBoardPieces()
        {
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
                if (move.IsPromotion(move, board, out message)) // initiate promotion sequence
                {
                    Pawn promotionPawn = (Pawn)move.GetPiece();
                    if (promotionPawn.GetColor() == MGChessLib.Common.Color.Light.ToString())
                    {
                        Popup lightPopup = LightPopup;
                        StackPanel panel = LightPanel;
                        panel.MouseDown += (s, e) => StackPanelMouseDown(s, e, move); // add the instance to event handler
                        lightPopup.PlacementTarget = ChessGrid.Children.OfType<Label>().First(x => x.Name == move.GetTargetSquare().GetName());
                        lightPopup.IsOpen = true;
                    }
                    else if (promotionPawn.GetColor() == MGChessLib.Common.Color.Dark.ToString())
                    {
                        Popup darkPopup = DarkPopup;
                        StackPanel panel = DarkPanel;
                        panel.MouseDown += (s, e) => StackPanelMouseDown(s, e, move); // overload event handler
                        darkPopup.PlacementTarget = ChessGrid.Children.OfType<Label>().First(x => x.Name == move.GetTargetSquare().GetName());
                        darkPopup.IsOpen = true;
                    }
                }
                else if(move.IsCastleMove(move, board, out message)) { lblStatus.Text = message; }
                board.SetMove(move, null);
                lblStatus.Text = message;
                ClearBoard();
                BoardToGUI();
            }
            else { lblStatus.Text = message; }
        }

        private void ClearBoard()
        {
            foreach (Label lbl in ChessGrid.Children.OfType<Label>()) { lbl.Content = null; }
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
            //lblStatus.Text = ((Image)e.OriginalSource).Name;
        }

        private void StackPanelMouseDown(object s, MouseButtonEventArgs e, Movement move)
        {
            Image image = (Image)e.OriginalSource;
            string color = move.GetPiece().GetColor();
            Piece piece = new Pawn(""); // temporary declare the piece as a colorless pawn
            if (color == MGChessLib.Common.Color.Light.ToString())
            {
                piece = (image.Name == "queen1") ? new Queen(color) : (image.Name == "rook1") ? new Rook(color) :
                        (image.Name == "bishop1") ? new Bishop(color) : new Knight(color);
            }
            else
            {
                piece = (image.Name == "queen2") ? new Queen(color) : (image.Name == "rook2") ? new Rook(color) :
                        (image.Name == "bishop2") ? new Bishop(color) : new Knight(color);
            }
            board.Promote(move, piece);
            DarkPopup.IsOpen = false;
            LightPopup.IsOpen = false;
            ClearBoard();
            BoardToGUI();
        }

        private void btnUndoClick(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, EventArgs e)
        {
            FirstWindow firstWindow = new FirstWindow();
            firstWindow.Show();
        }
    }
}
