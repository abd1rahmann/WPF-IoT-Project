using SimulatedFanApp.MVVM.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimulatedFanApp.MVVM.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             var fanWindow = new MainWindow();
             fanWindow.Show();
             Window.GetWindow(this)?.Close(); 
        }


        private void ExitButton_Click_1(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TopWindowBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Hämta fönstret som innehåller denna UserControl
            var window = Window.GetWindow(this);

            // Kontrollera om fönstret inte är null och anropa DragMove
            if (window != null)
            {
                window.DragMove();
            }
        }
     

       
    }
}
