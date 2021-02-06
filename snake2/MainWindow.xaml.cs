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
using System.Windows.Threading;

namespace Snake2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rectangle food;
        public DispatcherTimer timer; // this timer is public so we can start and stop it in case there is a new window set up
        private SettingsWindow setWindow;
        private SpeedSetting speedWindow;
        private Snake snakeySnake;
        public bool paintFood;

        public MainWindow()
        {
            //starts painting the form according to the code in xaml
            InitializeComponent();

            // starts up a timer that will manage the drawing of the snake
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(OnTickEventHandler);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            //start a new game
            NewGame();
        }

        /// <summary>
        /// This function handles the game logic.
        /// first check if the head is on the food, if so ad nother element.
        /// than - move the sanke.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTickEventHandler(object sender, EventArgs e)
        {
            snakeySnake.TicToc();
            PaintSnake();
            PaintFood();
        }
        /// <summary>
        /// this function handles painting the snake.
        /// to paint the snake we only need to paint 3 parts.
        /// The head - because its a different colour.
        /// the first in the tail,
        /// the last in the tail.
        /// all the rest dont need to be painted.
        /// </summary>
        private void PaintSnake()
        {
            for (int i = 0; i < snakeySnake.tail.Count; i++)
            {
                Canvas.SetTop(snakeySnake.tail[i].part, snakeySnake.tail[i].partPosition.Y);
                Canvas.SetLeft(snakeySnake.tail[i].part, snakeySnake.tail[i].partPosition.X);
            }
        }
        /// <summary>
        /// The function paints the food after its location has been updated.
        /// </summary>
        public void PaintFood()
        {
            if (paintFood)
            {
                Canvas.SetTop(food, snakeySnake.foodPosition.Y);
                Canvas.SetLeft(food, snakeySnake.foodPosition.X);
                paintFood = false;
            }
        }

        /// <summary>
        /// This function simply cleans the board.
        /// </summary>
        public void EndGame()
        {
            for (int i = 0; i < snakeySnake.tail.Count; i++)
            {
                paintCanvas.Children.Remove(snakeySnake.tail[i].part);
            }
            paintCanvas.Children.Remove(food);
            timer.Stop();
        }
        /// <summary>
        /// This function starts the game;
        /// it defines the head, the tail and the food.
        /// </summary>
        public void NewGame()
        {
            snakeySnake = new Snake(this, (int)(this.Height-100) ,(int)(this.Width - 100));
            // paint the snake head
            Canvas.SetTop(snakeySnake.head.part, snakeySnake.head.partPosition.Y);
            Canvas.SetLeft(snakeySnake.head.part, snakeySnake.head.partPosition.X);

            //start up the food and sets its position
            food = new Rectangle();
            food.Height = 10;
            food.Width = 10;
            food.Stroke = System.Windows.Media.Brushes.Black;
            food.Fill = System.Windows.Media.Brushes.Red;
            snakeySnake.NewFood();
            PaintFood();

            // actually adds the parts to the canvas
            paintCanvas.Children.Add(snakeySnake.head.part);
            paintCanvas.Children.Add(food);

        }
        /// <summary>
        /// a simple listiner for events of key down. it will update the vector accordingly after checking if we are not making illigal turn.
        /// illigal turn = turn the opposite diretion from our moving direction.
        /// If this is the first turn of the player, than it starts the timer that is responsible for moving the snake.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            snakeySnake.SnakeDirection(e);
        }
        /// <summary>
        /// A simple function that staarts a new game.
        /// first we clean the board.
        /// than we make a new game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            EndGame();
            NewGame();
        }
        /// <summary>
        /// event that happens when we click the settings window.
        /// open the setting window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_Settings_Size(object sender, RoutedEventArgs e)
        {
           setWindow= new SettingsWindow(this);
            timer.Stop();
            setWindow.Show();
        }

        /// <summary>
        /// event that happens when we click the settings window.
        /// open the setting window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_Settings_Speed(object sender, RoutedEventArgs e)
        {
            speedWindow = new SpeedSetting(this);
            timer.Stop();
            speedWindow.Show();
        }
        /// <summary>
        /// This function resets the size of the game board.
        /// NEEDS TO BE REFORMED AFTER DECOUPLING OF GAME LOGIC CLASS AND GUI. 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void SetGameSize(int height, int width)
        {
            this.Height = height;
            this.Width = width;
            paintCanvas.Height = this.Height - 100;
            paintCanvas.Width = this.Width - 100;
            snakeySnake.SetGameSize((int)(paintCanvas.Height) / 10, (int)(paintCanvas.Width) / 10);
            PaintFood();
            PaintSnake();
        }
    }
}
