using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Snake_Game.EventArgs;
using Key = Snake_Game.Models.Key;

namespace Snake_Game.ViewModels
{
    /// <summary>
    /// Main view model class
    /// </summary>
    public class MainViewModel : ObservableRecipient
    {
        /// <summary>
        /// Key pressed event
        /// </summary>
        public event EventHandler OnKeyPress;

        /// <summary>
        /// Window width
        /// </summary>
        public int WindowWidth => 256;

        /// <summary>
        /// Window height
        /// </summary>
        public int WindowHeight => 256;

        /// <summary>
        /// Main window and game board margin
        /// </summary>
        public int Margin => 8;

        /// <summary>
        /// Game board width
        /// </summary>
        public int GameBoardWidth => WindowWidth - 2 * Margin;

        /// <summary>
        /// Game board width
        /// </summary>
        public int GameBoardHeight => WindowHeight - 2 * Margin;

        /// <summary>
        /// Up key pressed command
        /// </summary>
        public ICommand UpKeyPressCommand { get; set; }

        /// <summary>
        /// Down key pressed command
        /// </summary>
        public ICommand DownKeyPressCommand { get; set; }

        /// <summary>
        /// Left key pressed command
        /// </summary>
        public ICommand LeftKeyPressCommand { get; set; }

        /// <summary>
        /// Right key pressed command
        /// </summary>
        public ICommand RightKeyPressCommand { get; set; }

        /// <summary>
        /// Escape key press
        /// </summary>
        public ICommand EscapeKeyPressCommand { get; set; }

        /// <summary>
        /// Enter key press
        /// </summary>
        public ICommand EnterKeyPressCommand { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            // Invoke OnKeyPress for Up
            UpKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() {Key = Key.Up});
            });

            // Invoke OnKeyPress for Down
            DownKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Key = Key.Down });
            });

            // Invoke OnKeyPress for Left
            LeftKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Key = Key.Left });
            });

            // Invoke OnKeyPress for Right
            RightKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Key = Key.Right });
            });

            // Invoke OnKeyPress for Enter
            EnterKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Key = Key.Enter });
            });

            // Kill the application
            EscapeKeyPressCommand = new RelayCommand(() =>
            {
                Application.Current.Shutdown();
            });
        }
    }
}
