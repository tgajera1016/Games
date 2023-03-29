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
    public class GameViewModel : ObservableRecipient
    {
        #region Private Properties

        /// <summary>
        /// Game board size
        /// </summary>
        public const int BoardSize = 512;

        /// <summary>
        /// Game model instance
        /// </summary>
        private readonly GameModel _gameModel = new ();

        /// <summary>
        /// Red brush
        /// </summary>
        private SolidColorBrush _redBrush = new (Colors.Red);

        /// <summary>
        /// Yellow brush
        /// </summary>
        private SolidColorBrush _yellowBrush = new(Colors.Yellow);

        #endregion

        #region Public Properties

        /// <summary>
        /// Game board size
        /// </summary>
        public System.Drawing.Size GameBoardSize => new(BoardSize, BoardSize);

        /// <summary>
        /// Snake body
        /// </summary>
        public ObservableCollection<Node> SnakeBody { get; set; } = new();

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainViewModel"></param>
        public GameViewModel(MainViewModel mainViewModel)
        {
            _gameModel.OnSnakeUpdate += OnSnakeUpdate;
            _gameModel.Start(GameBoardSize);

            if (mainViewModel != null)
                mainViewModel.OnKeyPress += OnKeyDown;
        }

        /// <summary>
        /// Update view based on the snake update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnSnakeUpdate(object sender, System.EventArgs eventArgs)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Move snake based on the key pressed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnKeyDown(object sender, System.EventArgs eventArgs)
        {
            if(eventArgs is KeyPressEventArgs keyPressEventArgs)
                _gameModel?.ChangeDirection(keyPressEventArgs.Direction);
        }

    }
}
