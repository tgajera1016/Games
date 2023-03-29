using System.Collections.Generic;
using Snake_Game.Models;

namespace Snake_Game.EventArgs
{
    /// <summary>
    /// Game model event args
    /// </summary>
    public class GameModelEventArgs : System.EventArgs
    {
        /// <summary>
        /// Snake node list
        /// </summary>
        public List<Node> SnakeNodeList { get; set; } = new();
    }
}
