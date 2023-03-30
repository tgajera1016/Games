using Snake_Game.Models;

namespace Snake_Game.EventArgs
{
    /// <summary>
    /// Key press event args class
    /// </summary>
    public class KeyPressEventArgs : System.EventArgs
    {
        /// <summary>
        /// Key
        /// </summary>
        public Key Key { get; set; }
    }
}
