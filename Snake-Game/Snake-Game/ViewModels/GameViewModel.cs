using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Snake_Game.EventArgs;
using Snake_Game.Models;
using Size = System.Drawing.Size;

namespace Snake_Game.ViewModels
{
    /// <summary>
    /// Game view model class
    /// </summary>
    public class GameViewModel : ObservableRecipient
    {
        #region Private Properties

        /// <summary>
        /// Game model instance
        /// </summary>
        private readonly GameModel _gameModel = new ();

        /// <summary>
        /// Red brush
        /// </summary>
        private readonly SolidColorBrush _redBrush = new (Colors.Red);

        /// <summary>
        /// Yellow brush
        /// </summary>
        private readonly SolidColorBrush _yellowBrush = new(Colors.Yellow);

        #endregion

        #region Public Properties

        /// <summary>
        /// Snake body
        /// </summary>
        public ObservableCollection<Node> SnakeBody { get; set; } = new();

        /// <summary>
        /// Game message
        /// </summary>
        private string _gameMessage = string.Empty;

        /// <summary>
        /// Game message property
        /// </summary>
        public string GameMessage
        {
            get => _gameMessage;
            set
            {
                _gameMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainViewModel"></param>
        public GameViewModel(MainViewModel mainViewModel)
        {
            _gameModel.GameStartEvent += OnGameStart;
            _gameModel.GameOverEvent += OnGameOver;
            _gameModel.SnakeUpdateEvent += OnSnakeUpdate;
            _gameModel.Start(new Size(mainViewModel.GameBoardWidth, mainViewModel.GameBoardHeight));

            mainViewModel.OnKeyPress += OnKeyDown;
        }

        /// <summary>
        /// Hide new game message when game starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnGameStart(object sender, System.EventArgs eventArgs)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                GameMessage = string.Empty;
            }));
        }

        /// <summary>
        /// Display new game message when game over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnGameOver(object sender, System.EventArgs eventArgs)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                GameMessage = "Press Enter to start a new game.";
            }));
        }

        /// <summary>
        /// Update view based on the snake update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnSnakeUpdate(object sender, System.EventArgs eventArgs)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (eventArgs is GameModelEventArgs gameModelEventArgs)
                {
                    var snakeNodeList = gameModelEventArgs.SnakeNodeList;
                    var nNodes = snakeNodeList.Count;
                    if (nNodes > 0)
                    {
                        SnakeBody.Clear();
                        for (var nodeIndex = 0; nodeIndex < nNodes; ++nodeIndex)
                        {
                            // Todo: do not change the color
                            snakeNodeList[nodeIndex].UpdateNode(nodeIndex == 0 ? _redBrush : _yellowBrush);
                            SnakeBody.Add(snakeNodeList[nodeIndex]);
                        }
                    }
                }
            }));
        }

        /// <summary>
        /// Move snake based on the key pressed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnKeyDown(object sender, System.EventArgs eventArgs)
        {
            if (eventArgs is KeyPressEventArgs keyPressEventArgs)
            {
                if (keyPressEventArgs.Key != Key.Enter)
                    _gameModel?.ChangeDirection(keyPressEventArgs.Key);
                else _gameModel.Restart();
            }
        }

    }
}
