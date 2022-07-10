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
    public partial class BoardTable : UserControl
    {
        private int _rows { get; set; }
        private int _columns { get; set; }
        private BoardCell[,] _board { get; set; }
        
        public ChessGame ChessGame { get; set; }

        public event PlayerMoveEventHandler PlayerMove;
        public delegate void PlayerMoveEventHandler(object sender, PlayerMoveEvent e);

        public BoardTable()
        {
            InitializeComponent();

            ChessGame = new();

            _rows = ChessGame.Board.Rows;
            _columns = ChessGame.Board.Columns;
            _board = new BoardCell[ChessGame.Board.Rows, ChessGame.Board.Columns];

            var pieces = ChessGame.Board.GetPieces();
            var isEmptyList = pieces.IsEmpty();
            Piece piece;

            for (int r = 0; r < ChessGame.Board.Rows; r++)
            {
                for (int c = 0; c < ChessGame.Board.Columns; c++)
                {
                    _board[r, c] = new(r, c, ChessGame);
                    _board[r, c].CellUpdate += BoardTable_CellUpdate;
                    _board[r, c].CasltingUpdate += BoardTable_CastlingUpdate;
                    _board[r, c].PlayerMove += BoardTable_PlayerMove;

                    if (!isEmptyList)
                    {
                        piece = pieces.FirstOrDefault(p => p.Position.Column == c && p.Position.Row == r);

                        if (piece != null)
                            _board[r, c].Piece = piece;
                    }
                }
            }

            InitializeBoard();
        }

        private void InitializeBoard()
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
                    _board[r, c].SetValue(Grid.RowProperty, r);
                    _board[r, c].SetValue(Grid.ColumnProperty, c);
                }
            }
        }

        private void BoardTable_CellUpdate(object sender, Models.Events.CellUpdateEvent e)
        {
            var boardCell = _board[e.Position.Row, e.Position.Column];
            boardCell.Piece = ChessGame.Board.GetPiece(boardCell.BoardPosition);
        }

        private void BoardTable_CastlingUpdate(object sender, Models.Events.CasltingUpdateEvent e)
        {
            var colorPieces = ChessGame.Board.GetPieces(e.Color).Where(p => p is King || p is Rook);

            foreach(Piece p in colorPieces)
            {
                var boardCell = _board[p.Position.Row, p.Position.Column];
                boardCell.Piece = ChessGame.Board.GetPiece(boardCell.BoardPosition);
            }
            int colorRow = colorPieces.FirstOrDefault().Position.Row;

            BoardCell cornerLeft = _board[colorRow, 0];
            BoardCell cornerRight = _board[colorRow, _columns - 1];

            cornerLeft.Piece = ChessGame.Board.GetPiece(cornerLeft.BoardPosition);
            cornerRight.Piece = ChessGame.Board.GetPiece(cornerRight.BoardPosition);
        }

        private void BoardTable_PlayerMove(object sender, PlayerMoveEvent e)
        {
            PlayerMove.Invoke(sender, e);
        }
    }
}
