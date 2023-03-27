using CommunityToolkit.Mvvm.ComponentModel;

namespace Snake_Game.ViewModels
{
    /// <summary>
    /// Main view model class
    /// </summary>
    public class MainViewModel : ObservableRecipient
    {
        public string TestText{ get; set; } = "This is a sample WPF application using CommunityToolkit and Ninject.";
    }
}
