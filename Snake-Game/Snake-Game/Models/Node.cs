using System.Windows.Media;

namespace Snake_Game.Models
{
    /// <summary>
    /// Node structure 
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public Node(int x, int y, int width, int height, SolidColorBrush color)
        {
            X = x;
            Y = y;
            Color = color;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Copy constructor 
        /// </summary>
        /// <param name="node"></param>
        public Node(Node node)
        {
            X = node.X;
            Y = node.Y;
            Color = node.Color;
            Width = node.Width;
            Height = node.Height;
        }

        /// <summary>
        /// Update node
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UpdateNode(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Update node
        /// </summary>
        /// <param name="color"></param>
        public void UpdateNode(SolidColorBrush color)
        {
            Color = color;
        }

        /// <summary>
        /// Update node
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public void UpdateNode(int x, int y, int width, int height, SolidColorBrush color)
        {
            X = x;
            Y = y;
            Color = color;
            Width = width;
            Height = height;
        }

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
    }
}
