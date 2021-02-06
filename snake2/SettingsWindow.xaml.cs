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

namespace Snake2
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MainWindow main; // use this parameter as referance to the main window
        public SettingsWindow(MainWindow reference)
        {
            this.main = reference;
            InitializeComponent();
        }
        /// <summary>
        /// sets the board size according to the players input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selected = (ComboBoxItem)sizeSelection.SelectedItem;
            string input = selected.Content.ToString();
            switch (input)
            {
                case "Large":
                    main.SetGameSize(750, 750);
                    break;
                case "Medium":
                    main.SetGameSize(550, 550);
                    break;
                case "Small":
                    main.SetGameSize(350, 350);
                    break;
            }
        }
        /// <summary>
        /// continues the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            main.timer.Start();
        }
    }
}
