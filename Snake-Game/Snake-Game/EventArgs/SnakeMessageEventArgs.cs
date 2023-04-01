using System.Collections.Generic;
using Snake_Game.Models;

namespace Snake_Game.EventArgs
{
    /// <summary>
    /// Snake message event args
    /// </summary>
    public class SnakeMessageEventArgs : System.EventArgs
    {
        /// <summary>
        /// Snake (list of nodes)
        /// </summary>
        public List<INode> Snake { get; set; }
    }
}
