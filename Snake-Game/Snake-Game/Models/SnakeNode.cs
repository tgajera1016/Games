using System.Windows.Media;

namespace Snake_Game.Models
{
    /// <summary>
    /// Snake node class
    /// </summary>
    public sealed class SnakeNode : INode
    {
        /// <summary>
        /// Node X coordinate
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Node Y Coordinate
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Node width
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Node height
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Node color
        /// </summary>
        public SolidColorBrush Color { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public SnakeNode(int x, int y, int width, int height, SolidColorBrush color)
        {
            X = x;
            Y = y;
            Color = color;
            Width = width;
            Height = height;
        }

        public void Move(int x, int y)
        {
            X += x;
            Y += y;
        }
    }
}
