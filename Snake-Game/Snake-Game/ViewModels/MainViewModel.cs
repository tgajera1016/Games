using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Snake_Game.EventArgs;

namespace Snake_Game.ViewModels
{
    /// <summary>
    /// Main view model class
    /// </summary>
    public class MainViewModel : ObservableRecipient
    {
        #region Public Properties

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

        #endregion

        #region Public Events

        /// <summary>
        /// Key pressed event
        /// </summary>
        public event EventHandler OnKeyPress;

        #endregion

        #region Public Commands

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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            // Invoke OnKeyPress for Up
            UpKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Key = Models.Key.Up });
            });

            // Invoke OnKeyPress for Down
            DownKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Key = Models.Key.Down });
            });

            // Invoke OnKeyPress for Left
            LeftKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Key = Models.Key.Left });
            });

            // Invoke OnKeyPress for Right
            RightKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Key = Models.Key.Right });
            });

            // Invoke OnKeyPress for Enter
            EnterKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Key = Models.Key.Enter });
            });

            // Kill the application
            EscapeKeyPressCommand = new RelayCommand(() =>
            {
                Application.Current.Shutdown();
            });
        }

        #endregion
    }
}
