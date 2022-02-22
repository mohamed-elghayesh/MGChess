using MGChessLib.Squares;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ChessWPF
{
    public class Helper
    {
        public static string GetFile(Label square)
        {
            return $"{square.Name[0]}";
        }

        public static string GetRank(Label square)
        {
            return $"{square.Name[1]}";
        }

        
    }
}
