using Ninject;
using Snake_Game.Modules;

namespace Snake_Game.ViewModels
{
    /// <summary>
    /// View model locator
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Kernel instance
        /// </summary>
        private static readonly IKernel Kernel;

        /// <summary>
        /// Constructor
        /// </summary>
        static ViewModelLocator()
        {
            Kernel = new StandardKernel(new Module());
        }

        /// <summary>
        /// Main view model
        /// </summary>
        public static MainViewModel MainViewModel => Kernel.Get<MainViewModel>();

        /// <summary>
        /// Game view model
        /// </summary>
        public static GameViewModel GameViewModel => Kernel.Get<GameViewModel>();
    }
}
