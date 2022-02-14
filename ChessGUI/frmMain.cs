using MGChessLib.Pieces;

namespace ChessGUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// the GUI can communicate with the library and send a control property "label.Name" that is altered in an event
        /// GameState.SetSelectedSquare, GameState.GetSelectedSquare
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            Pawn wP = new Pawn("w");
        }
    }
}