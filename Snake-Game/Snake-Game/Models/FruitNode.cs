﻿using System.Windows.Media;

namespace Snake_Game.Models
{
    /// <summary>
    /// Fruit node class
    /// </summary>
    public sealed class FruitNode : INode
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
        public string Color { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public FruitNode(int x, int y, int width, int height, string color)
        {
            X = x;
            Y = y;
            Color = color;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="existingFruitNode"></param>
        public FruitNode(FruitNode existingFruitNode)
        {
            X = existingFruitNode.X;
            Y = existingFruitNode.Y;
            Color = existingFruitNode.Color;
            Width = existingFruitNode.Width;
            Height = existingFruitNode.Height;
        }
    }
}
