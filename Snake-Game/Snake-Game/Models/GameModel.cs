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
        private const int SnakeStep = 1;

        /// <summary>
        /// Snake body
        /// </summary>
        private List<INode> Snake { get; } = new();

        /// <summary>
        /// Snake speed
        /// </summary>
        private readonly TimeSpan _snakeSpeed = TimeSpan.FromMilliseconds(10);

        /// <summary>
        /// Board size
        /// </summary>
        private System.Drawing.Size _boardSize = System.Drawing.Size.Empty;

        /// <summary>
        /// Current snake direction
        /// </summary>
        private Key _currentDirection = Key.Unknown;

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
            Snake.Add(GenerateRandomSnakeNode(Colors.Red));
            // Raise snake update event
            SnakeUpdateEvent?.Invoke(this, new SnakeMessageEventArgs { Snake = Snake });

            // Generate a fruit and raise fruit update event
            var fruitNode = GenerateRandomFruitNode(Colors.DarkOrange);
            FruitUpdateEvent?.Invoke(this, new FruitMessageEventArgs() { Fruit = fruitNode });
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
        }

        /// <summary>
        /// Generate random snake node
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private INode GenerateRandomSnakeNode(Color color)
        {
            var randomX = new Random().Next(NodeSize, _boardSize.Width - NodeSize);
            var randomY = new Random().Next(NodeSize, _boardSize.Height - NodeSize);
            return new SnakeNode(randomX, randomY, NodeSize, NodeSize, new SolidColorBrush(color));
        }

        /// <summary>
        /// Generate random fruit node
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private INode GenerateRandomFruitNode(Color color)
        {
            var randomX = new Random().Next(NodeSize, _boardSize.Width - NodeSize);
            var randomY = new Random().Next(NodeSize, _boardSize.Height - NodeSize);
            return new FruitNode(randomX, randomY, NodeSize, NodeSize, new SolidColorBrush(color));
            
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

                // Updates snake nodes based on the current direction 
                foreach (var node in Snake)
                    if(node is SnakeNode snakeNode)
                        snakeNode.Move(x, y);

                // Raise update snake event
                SnakeUpdateEvent?.Invoke(this, new SnakeMessageEventArgs { Snake = Snake });

                // Check for the collision 
                if (Snake.Count <= 0) return;
                if (!IsCollide(Snake[0])) return;

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

        /// <summary>
        /// Check whether snake is collide to the border
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsCollide(INode node)
        {
            try
            {
                // Check for the border collision 
                if (node is SnakeNode snakeNode)
                {
                    var x = snakeNode.X;
                    var y = snakeNode.Y;

                    if (x <= 1 || x >= _boardSize.Width - NodeSize)
                        return true;

                    if (y <= 1 || y >= _boardSize.Height - NodeSize)
                        return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return false;
        }

        #endregion
    }
}
