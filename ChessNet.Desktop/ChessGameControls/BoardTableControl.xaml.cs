using ChessNet.AI.RamdomInputsAI;
using ChessNet.Data.Interfaces;
using ChessNet.Data.Models;
using ChessNet.Data.Models.Pieces;
using ChessNet.Desktop.Models.Events;
using ChessNet.Utilities.Extensions;
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

namespace ChessNet.Desktop.ChessGameControls
{
    /// <summary>
    /// Interaction logic for BoardTable.xaml
    /// </summary>
    public partial class BoardTableControl : UserControl
    {
        private IPlayer _aiPlayer;

        private int _rows { get; set; }
        private int _columns { get; set; }
        private BoardCellControl[,] _board { get; set; }
        
        public ChessGame ChessGame { get; set; }

        public event EventHandler<PlayerMoveEvent> PlayerMove;

        public BoardTableControl(bool isColorInverted = true)
        {
            InitializeComponent();

            ChessGame = new();
            ChessGame.BoardUpdate += ChessGame_BoardUpdate;

            _rows = ChessGame.Board.Rows;
            _columns = ChessGame.Board.Columns;
            _board = new BoardCellControl[ChessGame.Board.Rows, ChessGame.Board.Columns];
            _aiPlayer = new RamdomAI(ChessGame, Data.Enums.PieceColor.Black);

            var pieces = ChessGame.Board.GetPieces();
            Piece piece;

            for (int r = 0; r < ChessGame.Board.Rows; r++)
            {
                for (int c = 0; c < ChessGame.Board.Columns; c++)
                {
                    _board[r, c] = new(new(c, r), ChessGame);
                    _board[r, c].PlayerMove += BoardTable_PlayerMove;           

                    piece = pieces.FirstOrDefault(p => p.Position.Column == c && p.Position.Row == r);

                    if (piece != null)
                        _board[r, c].Piece = piece;
                }
            }

            InitializeBoard(isColorInverted);
        }

        private void ChessGame_BoardUpdate(object? sender, Data.Models.Events.BoardUpdateEvent e)
        {
            _board[e.Position.Row, e.Position.Column].Piece = e.PieceAtPosition;
        }

        private void InitializeBoard(bool isInverted = true)
        {
            for (int i = 0; i < _columns; i++)
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < _rows; i++)
                BoardGrid.RowDefinitions.Add(new RowDefinition());

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    BoardGrid.Children.Add(_board[r, c]);
                    _board[r, c].SetValue(Grid.RowProperty, isInverted ? _rows - 1 - r : r);
                    _board[r, c].SetValue(Grid.ColumnProperty, c);
                }
            }
        }

        private void BoardTable_PlayerMove(object sender, PlayerMoveEvent e)
        {
            PlayerMove?.Invoke(sender, e);

            if (ChessGame.CurrentPlayer.Color == Data.Enums.PieceColor.Black &&
                !ChessGame.IsFinished)
            {
                var move = _aiPlayer.GetNextMove();

                try
                {
                    var player = ChessGame.CurrentPlayer;
                    var result = ChessGame.Move(move);

                    PlayerMove?.Invoke(this, new(result, player, ChessGame));
                }
                catch
                {
                    // TODO: Alerts and information.
                }
            }
        }
    }
}
