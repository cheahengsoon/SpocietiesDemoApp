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

namespace Spocieties
{
    /// <summary>
    /// Interaction logic for AgentWindow.xaml
    /// </summary>
    public partial class AgentWindow : Window
    {
        private MainWindow mainWindow;
        private AgentViewModel viewModel;

        public AgentWindow(MainWindow mw)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            mainWindow = mw;
            viewModel = new AgentViewModel();
            DataContext = viewModel;
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mainWindow.Visibility = System.Windows.Visibility.Visible;
            this.Close();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            mainWindow.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
