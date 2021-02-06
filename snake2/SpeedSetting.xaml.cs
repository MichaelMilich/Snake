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
    /// Interaction logic for SpeedSetting.xaml
    /// </summary>
    public partial class SpeedSetting : Window
    {
        private MainWindow main;// this parameter is used as refernce for the main window

        public SpeedSetting(MainWindow reference)
        {
            this.main = reference;
            InitializeComponent();
        }
        /// <summary>
        /// sets the timer interval according to the players request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selected = (ComboBoxItem)speedSelection.SelectedItem;
            string input = selected.Content.ToString();
            switch (input)
            {
                case "Slow":
                    main.timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                    break;
                case "Medium":
                    main.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                    break;
                case "Fast":
                    main.timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
                    break;
            }
            this.Close();
        }
        /// <summary>
        /// when this window closes, the game starts again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            main.timer.Start();
        }
    }
}
