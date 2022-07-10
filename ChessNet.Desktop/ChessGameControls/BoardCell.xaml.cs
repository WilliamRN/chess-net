using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;
using ChessNet.Desktop.Models.Events;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for BoardCell.xaml
    /// </summary>
    public partial class BoardCell : UserControl
    {
        private readonly ChessGame _chessGame;
        private MemoryStream _imageStream { get; set; }
        private BitmapImage _bitmapIimage { get; set; }
        private int _row { get; set; }
        private int _column { get; set; }

        public Piece _piece;
        public Piece Piece { get => _piece; set => SetPiece(value); }
        public BoardPosition BoardPosition { get; private set; }

        public event CellUpdateEventHandler CellUpdate;
        public delegate void CellUpdateEventHandler(object sender, CellUpdateEvent e);

        public BoardCell(int row, int column, ChessGame chessGame)
        {
            InitializeComponent();

            _row = row;
            _column = column;
            _chessGame = chessGame;
            BoardPosition = new(column, row);
        }

        public void Clear()
        {
            _piece = null;

            CellImage.Source = null;

            if (_bitmapIimage != null)
                _bitmapIimage.StreamSource.Dispose();
        }

        private void SetPiece(Piece piece)
        {
            if (piece is null)
            {
                Clear();
                return;
            }

            _piece = piece;

            byte[] image = Piece.GetTypeEnum() switch
            {
                PieceType.Pawn => _piece.IsWhite ? ChessNet.Resources.Images.W_Pawn : ChessNet.Resources.Images.B_Pawn,
                PieceType.King => _piece.IsWhite ? ChessNet.Resources.Images.W_King : ChessNet.Resources.Images.B_King,
                PieceType.Queen => _piece.IsWhite ? ChessNet.Resources.Images.W_Queen : ChessNet.Resources.Images.B_Queen,
                PieceType.Rook => _piece.IsWhite ? ChessNet.Resources.Images.W_Rook : ChessNet.Resources.Images.B_Rook,
                PieceType.Bishop => _piece.IsWhite ? ChessNet.Resources.Images.W_Bishop : ChessNet.Resources.Images.B_Bishop,
                PieceType.Knight => _piece.IsWhite ? ChessNet.Resources.Images.W_Knight : ChessNet.Resources.Images.B_Knight,
                _ => null,
            };

            _imageStream = new(image);
            _bitmapIimage = new BitmapImage();
            _bitmapIimage.BeginInit();
            _bitmapIimage.StreamSource = _imageStream;
            _bitmapIimage.EndInit();

            CellImage.Source = _bitmapIimage;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && this.Piece != null)
            {
                DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
            }
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(BoardCell));

            if (data is BoardCell cell)
            {
                BoardPosition from = new(cell._column, cell._row);
                BoardPosition to = new(this._column, this._row);

                try
                {
                    var result = _chessGame.Move(from, to);

                    if (result.IsValid)
                    {
                        CellUpdate.Invoke(this, new(from));
                        CellUpdate.Invoke(this, new(to));

                        if (result.IsCapture && result.CapturedPiece.Position != to)
                            CellUpdate.Invoke(this, new(result.CapturedPiece.Position));
                    }
                }
                catch
                {
                    // TODO: Alerts and information.
                }
            }
        }
    }
}
