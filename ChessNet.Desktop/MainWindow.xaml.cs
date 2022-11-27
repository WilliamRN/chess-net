using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Desktop.ChessGameControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessNet.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BoardTableControl _boardTableControl { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ResetBoard();
        }

        private void ResetBoard(bool isAI = false)
        {
            MainWindowGrid.Children.Clear();

            _boardTableControl = new(isAiOnly: isAI);
            _boardTableControl.PlayerMove += BoardTableContorl_PlayerMove;
            MainWindowGrid.Children.Add(_boardTableControl);

            Title = $"[ChessNet] Current Player: {_boardTableControl.ChessGame.CurrentPlayer.Color}";
        }

        private void BoardTableContorl_PlayerMove(object sender, Models.Events.PlayerMoveResultEvent e)
        {
            Dispatcher.Invoke(() =>
            {
                Title = $"[ChessNet] Current Player: {e.Player.Color}, " +
                    $"LastMove: {e.MoveResult.From.AsString()} to {e.MoveResult.To.AsString()}, " +
                    $"State: {e.State}";

                if (e.MoveResult.IsCapture)
                {
                    var captured = e.MoveResult.CapturedPiece;
                    Title += $", a {captured.Color} {captured.AsString()} was captured!";
                }

                if (e.MoveException != null)
                {
                    Title += $", Error: {e.MoveException.Message}";
                }
            });
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.R:
                    ResetBoard();
                    break;
                case Key.Y:
                    ResetBoard(true);
                    break;
            }
        }
    }
}
