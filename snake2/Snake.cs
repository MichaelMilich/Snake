using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Snake2
{
    class Snake
    {
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

        public SnakePart head;
        public List<SnakePart> tail;
        private direction movingdirection;
        private Point vector;
        private MainWindow main;
        private int gameHieght;
        private int gameWidth;
        public Point foodPosition;
        private bool isFirstTouch;

        public Snake(MainWindow reference, int hieght, int width)
        {
            main = reference;

            gameHieght = hieght;
            gameWidth = width;

            vector = new Point(10, 0);
            movingdirection = direction.right;
            isFirstTouch = true;
            // add the head of the snake and sets its position
            head = new SnakePart(new Point(200, 200));
            head.part.Fill = System.Windows.Media.Brushes.DarkSeaGreen;

            // add the tail for further use in the code
            tail = new List<SnakePart>();
            tail.Add(head);

            //start up the food and sets its position
            foodPosition = new Point();
            NewFood();

        }
        public void TicToc()
        {
            if (IsGameOn())
            {
                if (IsHeadOnFood())
                {
                    NewFood();
                    AddTail();
                }
                MoveSnake();
            }
            else
            {
                MessageBox.Show("Game ended! your score is :" + tail.Count);
                main.EndGame();
                main.NewGame();
            }
        }
        public void SnakeDirection(KeyEventArgs e)
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
                main.timer.Start();
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
        /// the function that checks if we didnt lose.
        /// the rules are:
        ///     1 - dont pass the borders.
        ///     2 - dont let the snake be on any of the parts.
        /// </summary>
        /// <returns>
        /// True - if the game is still playing
        /// False - if we lost the game
        /// </returns>
        public bool IsGameOn()
        {
            return !IsPastWalls() && !IsHeadOnTail();
        }
        /// <summary>
        /// checks the head snake location. 
        /// if the location is beyond the borders than we lost.
        /// </summary>
        /// <returns>
        /// true - beyond the borders, we lost
        /// false - inside the border, we still play.
        /// </returns>
        private bool IsPastWalls()
        {
            if ((head.partPosition.X + vector.X) > gameWidth || (head.partPosition.X + vector.X) < 0)
                return true;
            else if ((head.partPosition.Y + vector.Y) > gameHieght || (head.partPosition.Y + vector.Y) < 0)
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
        /// a simple function that checks if the head is on the food.
        /// </summary>
        /// <returns> true is the head is on the food
        /// false if it is not</returns>
        public bool IsHeadOnFood()
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
        public void AddTail()
        {
            tail.Add(new SnakePart(new Point(tail[tail.Count - 1].partPosition.X, tail[tail.Count - 1].partPosition.Y)));
            main.paintCanvas.Children.Add(tail[tail.Count - 1].part);

        }
        /// <summary>
        /// this function handles the moving of the snake.
        /// The function updates the location of each snake part in the list starting with the last.
        /// the last part is updated to the location of the part before and so on. This cleans the overlap of parts right after a new element was added.
        /// in th end, the head is updated to a new location according to the vector.
        /// </summary>
        public void MoveSnake()
        {
            for (int i = tail.Count - 1; i > 0; i--)
            {
                tail[i].partPosition.X = tail[i - 1].partPosition.X;
                tail[i].partPosition.Y = tail[i - 1].partPosition.Y;
            }
            head.partPosition.X += vector.X;
            head.partPosition.Y += vector.Y;
        }
        /// <summary>
        /// The function updates the food location randomly
        /// Add a check so the food is not placed on the snake.
        /// </summary>
        public void NewFood()
        {
            Random rnd = new Random();
            foodPosition.Y = rnd.Next(0, gameHieght / 10) * 10;
            foodPosition.X = rnd.Next(0, gameWidth / 10) * 10;
            main.paintFood = true;
        }
        /// <summary>
        /// The function places each position of the snake in the place acording to its relationship to the original size.
        /// in order to place the snake in the corect grid (multiples of 10), this function calculates everything in the pixels and than multiples by the pixel size (10)
        /// </summary>
        /// <param name="hieght"> actual hieght of the game board divided by 10</param>
        /// <param name="width"> actual width of the game board divided by 10</param>
        public void SetGameSize(int hieght, int width)
        {

            double x = 0;
            double y = 0;
            foreach (SnakePart part in tail)
            {
                x = part.partPosition.X / 10;
                part.partPosition.X = (int)((x / (gameWidth / 10)) * width) * 10;
                y = part.partPosition.Y / 10;
                part.partPosition.Y = (int)((y / (gameHieght / 10)) * hieght) * 10;
            }

            gameWidth = width * 10;
            gameHieght = hieght * 10;
            NewFood();
        }

    }
}
