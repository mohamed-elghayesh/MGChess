namespace MGChessLib
{
    public class Board
    {
        private static readonly int dimension = 8;
        private static readonly List<string> files = new() { "A", "B", "C", "D", "E", "F", "G", "H"};
        private static readonly List<string> ranks = new() { "1", "2", "3", "4", "5", "6", "7", "8" };
        private static readonly Dictionary<string, string> initialBoard = new Dictionary<string, string>()
        {
            {"A8", "bR"}, {"B8", "bN"}, {"C8", "bB"}, {"D8", "bQ"}, {"E8", "bK"}, {"F8", "bB"}, {"G8", "bN"}, {"H8", "bR"},
            {"A7", "bP"}, {"B7", "bP"}, {"C7", "bP"}, {"D7", "bP"}, {"E7", "bP"}, {"F7", "bP"}, {"G7", "bP"}, {"H7", "bP"},
            {"A6", "na"}, {"B6", "na"}, {"C6", "na"}, {"D6", "na"}, {"E6", "na"}, {"F6", "na"}, {"G6", "na"}, {"H6", "na"},
            {"A5", "na"}, {"B5", "na"}, {"C5", "na"}, {"D5", "na"}, {"E5", "na"}, {"F5", "na"}, {"G5", "na"}, {"H5", "na"},
            {"A4", "na"}, {"B4", "na"}, {"C4", "na"}, {"D4", "na"}, {"E4", "na"}, {"F4", "na"}, {"G4", "na"}, {"H4", "na"},
            {"A3", "na"}, {"B3", "na"}, {"C3", "na"}, {"D3", "na"}, {"E3", "na"}, {"F3", "na"}, {"G3", "na"}, {"H3", "na"},
            {"A2", "wP"}, {"B2", "wP"}, {"C2", "wP"}, {"D2", "wP"}, {"E2", "wP"}, {"F2", "wP"}, {"G2", "wP"}, {"H2", "wP"},
            {"A1", "wR"}, {"B1", "wN"}, {"C1", "wB"}, {"D1", "wQ"}, {"E1", "wK"}, {"F1", "wB"}, {"G1", "wN"}, {"H1", "wR"},
        };

        public static int Dimension()
        {
            return dimension;
        }
        public static List<string> Files()
        {
            return files;
        }
        public static List<string> Ranks()
        {
            return ranks;
        }
        public static Dictionary<string,string> InitBoard()
        {
            return initialBoard;
        } 
    }
}
