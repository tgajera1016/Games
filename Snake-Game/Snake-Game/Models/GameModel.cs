using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Snake_Game.EventArgs;

namespace Snake_Game.Models
{
    /// <summary>
    /// Game model class
    /// </summary>
    public class GameModel
    {
        #region Private Members

        /// <summary>
        /// Snake head size
        /// </summary>
        private const int NodeSize = 30;

        /// <summary>
        /// Number of steps snake takes at a time
        /// </summary>
        private const int SnakeStep = 30;

        /// <summary>
        /// Snake
        /// </summary>
        private List<INode> Snake { get; } = new();

        /// <summary>
        /// Fruit 
        /// </summary>
        private INode Fruit { get; set; }

        /// <summary>
        /// Snake speed
        /// </summary>
        private readonly TimeSpan _snakeSpeed = TimeSpan.FromMilliseconds(1000);

        /// <summary>
        /// Board size
        /// </summary>
        private System.Drawing.Size _boardSize = System.Drawing.Size.Empty;

        /// <summary>
        /// Current snake direction
        /// </summary>
        private Key _currentDirection = Key.Unknown;

        /// <summary>
        /// Snake color
        /// </summary>
        private readonly string _snakeColor = "#FF0000";

        /// <summary>
        /// Fruit color
        /// </summary>
        private readonly string _fruitColor = "#FFFF00";

        /// <summary>
        /// Cancellation token to stop updating snake display
        /// </summary>
        private static readonly CancellationTokenSource SnakeUpdateCancellationTokenSource = new();
        private readonly CancellationToken _snakeUpdateCancellationToken = SnakeUpdateCancellationTokenSource.Token;

        #endregion

        #region Public EventHandler

        /// <summary>
        /// Public event on fruit update
        /// </summary>
        public event EventHandler<System.EventArgs> FruitUpdateEvent;

        /// <summary>
        /// Public event on snake update
        /// </summary>
        public event EventHandler<System.EventArgs> SnakeUpdateEvent;

        /// <summary>
        /// Public event on game over
        /// </summary>
        public event EventHandler<System.EventArgs> GameOverEvent;

        /// <summary>
        /// Public event on game start
        /// </summary>
        public event EventHandler GameStartEvent;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public GameModel()
        {
            UpdateSnake();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Stop game
        /// </summary>
        public void Stop()
        {
            Reset();
        }

        /// <summary>
        /// Start game
        /// </summary>
        /// <param name="boardSize"></param>
        public void Start(System.Drawing.Size boardSize)
        {
            _boardSize = boardSize;

            // Initialize game
            Initialize();
        }

        /// <summary>
        /// Restart the game
        /// </summary>
        public void Restart()
        {
            Initialize();
        }

        /// <summary>
        /// Change direction of the snake
        /// </summary>
        /// <param name="direction"></param>
        public void ChangeDirection(Key direction)
        {
            _currentDirection = direction;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize game
        /// </summary>
        private void Initialize()
        {
            // Raise game start event
            GameStartEvent?.Invoke(this, System.EventArgs.Empty);

            // Clear snake
            Snake.Clear();
            // Add a random node to snake
            Snake.Insert(0, GenerateRandomSnakeNode(_snakeColor));
            // Raise snake update event
            SnakeUpdateEvent?.Invoke(this, new SnakeMessageEventArgs { Snake = Snake });

            // Generate a fruit and raise fruit update event
            Fruit = GenerateRandomFruitNode(_fruitColor);
            FruitUpdateEvent?.Invoke(this, new FruitMessageEventArgs { Fruit = Fruit });
        }

        /// <summary>
        /// Reset game model
        /// </summary>
        private void Reset()
        {
            // Update current direction to unknown 
            _currentDirection = Key.Unknown;

            // Clear snake node list
            Snake.Clear();
            // Raise snake update event
            SnakeUpdateEvent?.Invoke(this, new SnakeMessageEventArgs { Snake = Snake });

            // Clear the fruit 
            Fruit = null;
            // Raise fruit update event
            FruitUpdateEvent?.Invoke(this, new FruitMessageEventArgs { Fruit = Fruit});

        }

        /// <summary>
        /// Generate random snake node
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private INode GenerateRandomSnakeNode(string color)
        {
            var randomX = new Random().Next(NodeSize, _boardSize.Width - NodeSize);
            var randomY = new Random().Next(NodeSize, _boardSize.Height - NodeSize);
            return new SnakeNode(randomX, randomY, NodeSize, NodeSize, color);
        }

        /// <summary>
        /// Generate random fruit node
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private INode GenerateRandomFruitNode(string color)
        {
            var randomX = new Random().Next(NodeSize, _boardSize.Width - NodeSize);
            var randomY = new Random().Next(NodeSize, _boardSize.Height - NodeSize);
            return new FruitNode(randomX, randomY, NodeSize, NodeSize, color);
        }

        /// <summary>
        /// Update snake
        /// </summary>
        private void UpdateSnake()
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    while (!_snakeUpdateCancellationToken.IsCancellationRequested)
                    {
                        switch (_currentDirection)
                        {
                            case Key.Left:
                                UpdateSnake(-SnakeStep, 0);
                                break;
                            case Key.Right:
                                UpdateSnake(SnakeStep, 0);
                                break;
                            case Key.Up:
                                UpdateSnake(0, -SnakeStep);
                                break;
                            case Key.Down:
                                UpdateSnake(0, SnakeStep);
                                break;
                            case Key.Unknown:
                            default:
                                break;
                        }

                        await Task.Delay(_snakeSpeed, _snakeUpdateCancellationToken);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }, _snakeUpdateCancellationToken);
        }

        /// <summary>
        /// Update snake position 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void UpdateSnake(int x, int y)
        {
            try
            {
                // Do not update snake if key is not pressed
                if (x == 0 && y == 0)
                    return;

                var nNodes = Snake.Count;
                if (nNodes <= 0) return;

                // Updates snake nodes based on the current direction 
                for (var nodeIndex = nNodes - 1; nodeIndex > 0; --nodeIndex)
                    SwapCoordinates(Snake[nodeIndex-1], Snake[nodeIndex]);

                ((SnakeNode)Snake[0]).Move(x, y);

                // Raise update snake event
                SnakeUpdateEvent?.Invoke(this, new SnakeMessageEventArgs { Snake = Snake });

                // Check if fruit can be eaten
                if (CanEatFruit(Snake[0], Fruit))
                {
                    if (Fruit is FruitNode fruitNode)
                    {
                        // Add the last fruit location in front of the snake list and update the rest of the nodes
                        Snake.Insert(0, new SnakeNode(fruitNode.X, fruitNode.Y, fruitNode.Width, fruitNode.Height, fruitNode.Color));

                        // Raise update snake event
                        SnakeUpdateEvent?.Invoke(this, new SnakeMessageEventArgs { Snake = Snake });
                    }

                    // Generate a new fruit node
                    Fruit = GenerateRandomFruitNode(_fruitColor);
                    // Raise update fruit event
                    FruitUpdateEvent?.Invoke(this, new FruitMessageEventArgs {Fruit = Fruit});
                }

                // Check for the collision 
                if (!IsCollideWithBorder(Snake[0], _boardSize)) return;

                // Reset game if collide
                Reset();
                // Raise game over event 
                GameOverEvent?.Invoke(this, System.EventArgs.Empty);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void SwapCoordinates(INode sourceNode, INode destinationNode)
        {
            if (sourceNode is not SnakeNode sourceSnakeNode) return;
            if (destinationNode is not SnakeNode destinationSnakeNode) return;
            destinationSnakeNode.Update(sourceSnakeNode.X, sourceSnakeNode.Y);
        }

        /// <summary>
        /// Check whether snake is collide to the border
        /// </summary>
        /// <param name="node"></param>
        /// <param name="borderSize"></param>
        /// <returns></returns>
        private bool IsCollideWithBorder(INode node, System.Drawing.Size borderSize)
        {
            try
            {
                // Check for the border collision 
                if (node is SnakeNode snakeNode)
                {
                    var x = snakeNode.X;
                    var y = snakeNode.Y;

                    if (x <= 1 || x >= borderSize.Width - NodeSize)
                        return true;

                    if (y <= 1 || y >= borderSize.Height - NodeSize)
                        return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return false;
        }

        /// <summary>
        /// Check weather it can eat fruit or not
        /// </summary>
        /// <returns></returns>
        private bool CanEatFruit(INode snakeNode, INode fruitNode)
        {
            if (snakeNode is not SnakeNode sNode) return false;
            if (fruitNode is not FruitNode fNode) return false;

             var distance = Math.Sqrt((sNode.X - fNode.X) * (sNode.X - fNode.X) + (sNode.Y - fNode.Y) * (sNode.Y - fNode.Y));
            return distance <= NodeSize;
        }

        #endregion
    }
}
