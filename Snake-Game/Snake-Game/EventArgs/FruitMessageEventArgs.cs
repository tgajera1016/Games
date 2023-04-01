using Snake_Game.Models;

namespace Snake_Game.EventArgs
{
    /// <summary>
    /// fruit message event args
    /// </summary>
    public class FruitMessageEventArgs : System.EventArgs
    {
        /// <summary>
        /// Fruit node
        /// </summary>
        public INode Fruit { get; set; }
    }
}
