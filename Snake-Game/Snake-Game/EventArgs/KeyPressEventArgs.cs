using Snake_Game.Models;

namespace Snake_Game.EventArgs
{
    /// <summary>
    /// Key press event args class
    /// </summary>
    public class KeyPressEventArgs : System.EventArgs
    {
        /// <summary>
        /// Direction
        /// </summary>
        public Direction Direction { get; set; }
    }
}
