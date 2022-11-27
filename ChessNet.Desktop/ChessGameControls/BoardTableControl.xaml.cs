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
        private IPlayer _aiPlayerBlack;
        private IPlayer _aiPlayerWhite;
        private const int AI_DELAY = 200;

        private bool _isAiOnly { get; set; }
        private int _rows { get; set; }
        private int _columns { get; set; }
        private BoardCellControl[,] _board { get; set; }
        
        public ChessGame ChessGame { get; set; }

        public event EventHandler<PlayerMoveResultEvent> PlayerMove;

        public BoardTableControl(bool isColorInverted = true, bool isAiOnly = false)
        {
            InitializeComponent();

            ChessGame = new();
            ChessGame.BoardUpdate += ChessGame_BoardUpdate;

            _isAiOnly = isAiOnly;
            _rows = ChessGame.Board.Rows;
            _columns = ChessGame.Board.Columns;
            _board = new BoardCellControl[_rows, _columns];
            _aiPlayerBlack = new RamdomAI(ChessGame, Data.Enums.PieceColor.Black);
            _aiPlayerWhite = new RamdomAI(ChessGame, Data.Enums.PieceColor.White);

            InitializeCellControls();
            InitializeBoardGrid(isColorInverted);

            if (_isAiOnly)
                Task.Run(NextAIMove);
        }

        private void InitializeCellControls()
        {
            var pieces = ChessGame.Board.GetPieces();
            Piece piece;

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    _board[r, c] = new(new(c, r), ChessGame);
                    _board[r, c].CellMove += BoardTableControl_CellMove;

                    piece = pieces.FirstOrDefault(p => p.Position.Column == c && p.Position.Row == r);

                    if (piece != null)
                        _board[r, c].Piece = piece;
                }
            }
        }

        private void InitializeBoardGrid(bool isInverted = true)
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

        private void BoardTableControl_CellMove(object? sender, CellMoveEvent e)
        {
            try
            {
                var result = ChessGame.Move(e.Move);
                PlayerMove?.Invoke(this, new(result, e.Player, ChessGame));
            }
            catch (Exception ex)
            {
                PlayerMove?.Invoke(this, new(ex, e.Player, ChessGame));
            }

            Task.Run(NextAIMove);
        }

        private void NextAIMove()
        {
            IPlayer player = ChessGame.CurrentPlayer.Color == Data.Enums.PieceColor.Black
                ? _aiPlayerBlack : _isAiOnly ? _aiPlayerWhite : null;

            if (player != null && !ChessGame.IsFinished)
            {
                if (_isAiOnly)
                    Task.Delay(AI_DELAY).Wait();

                var move = player.GetNextMove();
                BoardTableControl_CellMove(this, new(ChessGame.CurrentPlayer, move));
            }
        }

        private void ChessGame_BoardUpdate(object? sender, Data.Models.Events.BoardUpdateEvent e)
        {
            _board[e.Position.Row, e.Position.Column].Piece = e.PieceAtPosition;
        }
    }
}
