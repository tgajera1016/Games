using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Snake_Game.EventArgs;
using Snake_Game.Models;

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
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            // Invoke OnKeyPress for Up
            UpKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() {Direction = Direction.Up});
            });

            // Invoke OnKeyPress for Down
            DownKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Direction = Direction.Down });
            });

            // Invoke OnKeyPress for Left
            LeftKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Direction = Direction.Left });
            });

            // Invoke OnKeyPress for Right
            RightKeyPressCommand = new RelayCommand(() =>
            {
                OnKeyPress?.Invoke(this, new KeyPressEventArgs() { Direction = Direction.Right });
            });
        }
    }
}
