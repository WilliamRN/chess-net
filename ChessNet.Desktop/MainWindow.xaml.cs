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
        private BoardTable _boardTable { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ResetBoard();
        }

        private void ResetBoard()
        {
            MainWindowGrid.Children.Clear();

            _boardTable = new();
            _boardTable.PlayerMove += _boardTable_PlayerMove;
            MainWindowGrid.Children.Add(_boardTable);

            Title = $"[ChessNet] Current Player: White";
        }

        private void _boardTable_PlayerMove(object sender, Models.Events.PlayerMoveEvent e)
        {
            Title = $"[ChessNet] Current Player: {_boardTable.ChessGame.CurrentPlayer.Color}, " +
                $"LastMove: {e.MoveResult.From.AsString()} to {e.MoveResult.To.AsString()}, " +
                $"State: {_boardTable.ChessGame.State}";

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
