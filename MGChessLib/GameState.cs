

namespace MGChessLib
{
    public class GameState
    {
        Dictionary<string, string> gameBoard = Board.InitBoard();

        public Dictionary<string, string> GetGameBoard()
        {
            return gameBoard;
        }
    }
}
