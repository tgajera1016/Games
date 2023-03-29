using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Snake_Game.EventArgs;

namespace Snake_Game.Models
{
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
        private List<Node> Snake { get; } = new();

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
        private Direction _currentDirection = Direction.Unknown;

        /// <summary>
        /// Cancellation token to stop updating snake display
        /// </summary>
        private static readonly CancellationTokenSource SnakeUpdateCancellationTokenSource = new();
        private readonly CancellationToken _snakeUpdateCancellationToken = SnakeUpdateCancellationTokenSource.Token;

        #endregion

        #region Public EventHandler

        /// <summary>
        /// Public event on snake update
        /// </summary>
        public event EventHandler<System.EventArgs> OnSnakeUpdate;

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
            UpdateSnake();

            _boardSize = boardSize;

            Snake.Clear();
            Snake.Add(GenerateRandomNode(Colors.Red));
            OnSnakeUpdate?.Invoke(this, new GameModelEventArgs { SnakeNodeList = Snake });

        }

        /// <summary>
        /// Change direction of the snake
        /// </summary>
        /// <param name="direction"></param>
        public void ChangeDirection(Direction direction)
        {
            _currentDirection = direction;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Reset game model
        /// </summary>
        private void Reset()
        {
            Snake.Clear();
            Snake.Add(new Node(-100, -100, NodeSize, NodeSize, new SolidColorBrush(Colors.Red)));
            OnSnakeUpdate?.Invoke(this, new GameModelEventArgs { SnakeNodeList = Snake });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Node GenerateRandomNode(Color color)
        {
            var randomX = new Random().Next(NodeSize, _boardSize.Width - NodeSize);
            var randomY = new Random().Next(NodeSize, _boardSize.Height - NodeSize);
            return new Node(randomX, randomY, NodeSize, NodeSize, new SolidColorBrush(color));
        }



        /// <summary>
        /// Update snake
        /// </summary>
        private void UpdateSnake()
        {
            _ = Task.Run(async () =>
            {
                while (!_snakeUpdateCancellationToken.IsCancellationRequested)
                {
                    switch (_currentDirection)
                    {
                        case Direction.Left:
                            UpdateSnake(-SnakeStep, 0);
                            break;
                        case Direction.Right:
                            UpdateSnake(SnakeStep, 0);
                            break;
                        case Direction.Up:
                            UpdateSnake(0, -SnakeStep);
                            break;
                        case Direction.Down:
                            UpdateSnake(0, SnakeStep);
                            break;
                        case Direction.Unknown:
                        default:
                            UpdateSnake(0, 0);
                            break;
                    }

                    await Task.Delay(_snakeSpeed, _snakeUpdateCancellationToken);
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
                if (x == 0 && y == 0)
                    return;

                var nNodes = Snake.Count;
                if (nNodes == 0)
                    return;

                for (var nodeIndex = 1; nodeIndex < nNodes; ++nodeIndex)
                {
                    Snake[nodeIndex] = Snake[nodeIndex - 1];
                    Snake[nodeIndex].UpdateNode(new SolidColorBrush(Colors.Yellow));
                }

                var newX = Snake[0].X + x;
                var newY = Snake[0].Y + y;

                Snake[0].UpdateNode(newX, newY);
                Snake[0].UpdateNode(new SolidColorBrush(Colors.Red));
                OnSnakeUpdate?.Invoke(this, new GameModelEventArgs { SnakeNodeList = Snake });

                if (IsCollide(newX, newY))
                {
                    // Todo: reset the game or restart
                    _currentDirection = Direction.Unknown;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Check whether snake is collide to the border
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsCollide(int x, int y)
        {
            if (x <= 1 || x >= _boardSize.Width - NodeSize - 1)
                return true;

            if (y <= 1 || y >= _boardSize.Height - NodeSize - 1)
                return true;

            return false;
        }

        #endregion
    }
}
