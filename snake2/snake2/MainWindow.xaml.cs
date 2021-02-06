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
        private SnakePart head;
        private List<SnakePart> tail;
        private direction movingdirection = direction.right;
        private Point vector = new Point(10, 0);
        private Rectangle food;
        private Point foodPosition = new Point();
        private DispatcherTimer timer;
        private bool isFirstTouch = true;

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
            if (isGameOn())
            {
                if (isHeadOnFood())
                {
                    newFood();
                    addTail();
                }
                moveSnake();
            }
            else
            {
                MessageBox.Show("Game ended! your score is :" + tail.Count);
                EndGame();
                NewGame();
            }
        }
        /// <summary>
        /// this function handles the moving of the snake.
        /// The function updates the location of each snake part in the list starting with the last.
        /// the last part is updated to the location of the part before and so on. This cleans the overlap of parts right after a new element was added.
        /// in th end, the head is updated to a new location according to the vector.
        /// </summary>
        private void moveSnake()
        {
            for (int i = tail.Count - 1; i > 0; i--)
            {
                tail[i].partPosition.X = tail[i - 1].partPosition.X;
                tail[i].partPosition.Y = tail[i - 1].partPosition.Y;
                Canvas.SetTop(tail[i].part, tail[i].partPosition.Y); // better to paint only the last paint and the head.
                Canvas.SetLeft(tail[i].part, tail[i].partPosition.X);
            }
            head.partPosition.X += vector.X;
            head.partPosition.Y += vector.Y;
            Canvas.SetTop(tail[0].part, tail[0].partPosition.Y);
            Canvas.SetLeft(tail[0].part, tail[0].partPosition.X);
        }
        /// <summary>
        /// a simple function that checks if the head is on the food.
        /// </summary>
        /// <returns> true is the head is on the food
        /// false if it is not</returns>
        public bool isHeadOnFood()
        {
            return head.partPosition.Equals(foodPosition);
        }
        /// <summary>
        /// A simple function that addes another snake part to the tail.
        /// the procedure is easy:
        /// add a new snake part to the list with the same location as the last part in the tail.
        /// add the NEW last part in the list to the canvas.
        /// This function is called before moving the snake, once the snake is moved, there will be no overlaping snakeParts
        /// IN THE FUTURE MAKE SURE THERE ARE NO OVERLAPING SNAKEPARTS AS IS
        /// </summary>
        public void addTail()
        {
            tail.Add(new SnakePart(new Point(tail[tail.Count - 1].partPosition.X, tail[tail.Count - 1].partPosition.Y)));
            paintCanvas.Children.Add(tail[tail.Count - 1].part);

        }
        /// <summary>
        /// The function updates the food location randomly
        /// Add a check so the food is not placed on the snake.
        /// </summary>
        public void newFood()
        {
            Random rnd = new Random();
            foodPosition.Y = rnd.Next(0, 64) * 10;
            Canvas.SetTop(food, foodPosition.Y);
            foodPosition.X = rnd.Next(0, 64) * 10;
            Canvas.SetLeft(food, foodPosition.X);
        }
        /// <summary>
        /// the function that checks if we didnt lose.
        /// the rules are:
        ///     1 - dont pass the borders.
        ///     2 - dont let the snake be on any of the parts.
        /// </summary>
        /// <returns>
        /// True - if the game is still playing
        /// False - if we lost the game
        /// </returns>
        private bool isGameOn()
        {
            return !isPastWalls() && !IsHeadOnTail();
        }
        /// <summary>
        /// checks the head snake location. 
        /// if the location is beyond the borders than we lost.
        /// </summary>
        /// <returns>
        /// true - beyond the borders, we lost
        /// false - inside the border, we still play.
        /// </returns>
        private bool isPastWalls()
        {
            if ((head.partPosition.X + vector.X) > 640 || (head.partPosition.X + vector.X) < 0)
                return true;
            else if ((head.partPosition.Y + vector.Y) > 640 || (head.partPosition.Y + vector.Y) < 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// checks the head snake location. 
        /// if the location is on any of the tail - we lost
        /// </summary>
        /// <returns>
        /// true - head is on the tail, we lost
        /// false - head is not on the tail, we still play.
        /// </returns>
        private bool IsHeadOnTail()
        {
            for (int i = 1; i < tail.Count; i++)
            {
                if (head.partPosition.Equals(tail[i].partPosition))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// This function simply cleans the board.
        /// </summary>
        private void EndGame()
        {
            for (int i = 0; i < tail.Count; i++)
            {
                paintCanvas.Children.Remove(tail[i].part);
            }
            paintCanvas.Children.Remove(food);
            timer.Stop();
            isFirstTouch = true;
        }
        /// <summary>
        /// This function starts the game;
        /// it defines the head, the tail and the food.
        /// </summary>
        private void NewGame()
        {
            // add the head of the snake and sets its position
            head = new SnakePart(new Point(200, 200));
            head.part.Fill = System.Windows.Media.Brushes.DarkSeaGreen;
            Canvas.SetTop(head.part, head.partPosition.Y);
            Canvas.SetLeft(head.part, head.partPosition.X);

            // add the tail for further use in the code
            tail = new List<SnakePart>();
            tail.Add(head);

            //start up the food and sets its position
            food = new Rectangle();
            food.Height = 10;
            food.Width = 10;
            food.Stroke = System.Windows.Media.Brushes.Black;
            food.Fill = System.Windows.Media.Brushes.Red;
            newFood();

            // actually adds the parts to the canvas
            paintCanvas.Children.Add(head.part);
            paintCanvas.Children.Add(food);

        }
        /// <summary>
        /// the direction the snake can go.
        /// </summary>
        enum direction
        {
            up,
            left,
            right,
            down
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
            // check if its the first key. if so, than set the diretion accordingly.
            if (isFirstTouch)
            {
                isFirstTouch = false;
                switch (e.Key)
                {
                    case Key.Down:
                        movingdirection = direction.down;
                        vector.X = 0;
                        vector.Y = 10;
                        break;
                    case Key.Up:
                        movingdirection = direction.up;
                        vector.X = 0;
                        vector.Y = -10;
                        break;
                    case Key.Right:
                        movingdirection = direction.right;
                        vector.X = 10;
                        vector.Y = 0;
                        break;
                    case Key.Left:
                        movingdirection = direction.left;
                        vector.X = -10;
                        vector.Y = 0;
                        break;
                }
                timer.Start();
            }
            else // if its not the first key, go with the game logic.
            {
                switch (e.Key)
                {
                    case Key.Down:
                        if (movingdirection != direction.up)
                        {
                            movingdirection = direction.down;
                            vector.X = 0;
                            vector.Y = 10;
                        }
                        break;
                    case Key.Up:
                        if (movingdirection != direction.down)
                        {
                            movingdirection = direction.up;
                            vector.X = 0;
                            vector.Y = -10;
                        }
                        break;
                    case Key.Right:
                        if (movingdirection != direction.left)
                        {
                            movingdirection = direction.right;
                            vector.X = 10;
                            vector.Y = 0;
                        }
                        break;
                    case Key.Left:
                        if (movingdirection != direction.right)
                        {
                            movingdirection = direction.left;
                            vector.X = -10;
                            vector.Y = 0;
                        }
                        break;
                }
            }

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
    }
}
