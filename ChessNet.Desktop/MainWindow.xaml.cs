using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Desktop.ChessGameControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void ResetBoard()
        {
            MainWindowGrid.Children.Clear();

            _boardTableControl = new();
            _boardTableControl.PlayerMove += _boardTable_PlayerMove;
            MainWindowGrid.Children.Add(_boardTableControl);

            Title = $"[ChessNet] Current Player: {_boardTableControl.ChessGame.CurrentPlayer.Color}";
        }

        private void _boardTable_PlayerMove(object sender, Models.Events.PlayerMoveEvent e)
        {
            Title = $"[ChessNet] Current Player: {e.Player.Color}, " +
                $"LastMove: {e.MoveResult.From.AsString()} to {e.MoveResult.To.AsString()}, " +
                $"State: {e.State}";

            if (e.MoveResult.IsCapture)
            {
                var captured = e.MoveResult.CapturedPiece;
                Title += $", a {captured.Color} {captured.AsString()} was captured!";
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                ResetBoard();
            }
        }
    }
}
