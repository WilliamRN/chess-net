using ChessNet.Data.Models;
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

        public BoardTable(int rows, int columns, IEnumerable<Piece> pieces = null)
        {
            InitializeComponent();
            _rows = rows;
            _columns = columns;
            _board = new BoardCell[_rows, _columns];

            bool isListEmpty = pieces.IsEmpty();
            Piece piece;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _board[r, c] = new();

                    if (!isListEmpty)
                    {
                        piece = pieces.FirstOrDefault(p => p.Position.Column == c && p.Position.Row == r);

                        if (piece != null)
                            _board[r, c].SetPiece(piece);
                    }
                }
            }

            InitializeBoard();
        }

        public BoardTable(int size, IEnumerable<Piece> pieces = null) : this(size, size, pieces)
        {

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
    }
}
