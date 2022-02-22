using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessWPF
{
    /// <summary>
    /// Interaction logic for FirstWindow.xaml
    /// </summary>
    public partial class FirstWindow : Window
    {
        public FirstWindow()
        {
            InitializeComponent();
        }

        private void RandomEngineButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            

            TwoPlayersWindow window = new TwoPlayersWindow();
            window.Show();
            this.Close();
        }
    }
}
