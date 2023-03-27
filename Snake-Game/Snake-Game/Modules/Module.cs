using Ninject.Modules;
using Snake_Game.ViewModels;

namespace Snake_Game.Modules
{
    /// <summary>
    /// View model modules
    /// </summary>
    public class Module : NinjectModule
    {
        /// <summary>
        /// Load view model modules required for the application
        /// </summary>
        public override void Load()
        {
            Bind<MainViewModel>().ToSelf().InSingletonScope();
        }
    }
}
