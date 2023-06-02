using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Snake_Game.EventArgs;
using Snake_Game.Models;

namespace Snake_Game.ViewModels
{
    /// <summary>
    /// Game view model class
    /// </summary>
    public class GameViewModel : ObservableObject
    {
        #region Private Properties

        /// <summary>
        /// Game model instance
        /// </summary>
        private readonly GameModel _gameModel = new ();

        #endregion

        #region Public Properties

        /// <summary>
        /// Snake
        /// </summary>
        private ObservableCollection<INode> _snake = new ();
        public ObservableCollection<INode> Snake
        {
            get => _snake;
            set
            {
                if (value == null) return;
                _snake = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Fruit
        /// </summary>
        private ObservableCollection<INode> _fruit = new();
        public ObservableCollection<INode> Fruit
        {
            get => _fruit;
            set
            {
                if (value == null) return;
                _fruit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Game message
        /// </summary>
        private string _gameMessage = string.Empty;
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

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainViewModel"></param>
        public GameViewModel(MainViewModel mainViewModel)
        {
            _gameModel.GameStartEvent += OnGameStart;
            _gameModel.GameOverEvent += OnGameOver;
            _gameModel.SnakeUpdateEvent += OnSnakeUpdate;
            _gameModel.FruitUpdateEvent += OnFruitUpdate;
            _gameModel.Start(new System.Drawing.Size(mainViewModel.GameBoardWidth, mainViewModel.GameBoardHeight));

            mainViewModel.OnKeyPress += OnKeyDown;
        }

        #endregion

        #region Private Methods

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
                try
                {
                    if (eventArgs is not SnakeMessageEventArgs snakeMessageEventArgs) return;
                    Snake = snakeMessageEventArgs.Snake == null
                        ? new ObservableCollection<INode>()
                        : new ObservableCollection<INode>(snakeMessageEventArgs.Snake);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }));
        }

        /// <summary>
        /// Update view based on the fruit update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnFruitUpdate(object sender, System.EventArgs eventArgs)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (eventArgs is not FruitMessageEventArgs fruitMessageEventArgs) return;
                    Fruit = fruitMessageEventArgs.Fruit == null
                        ? new ObservableCollection<INode>()
                        : new ObservableCollection<INode> { fruitMessageEventArgs.Fruit };
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
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

        #endregion
    }
}
