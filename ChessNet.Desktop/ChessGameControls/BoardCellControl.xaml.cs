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
    public partial class BoardCellControl : UserControl
    {
        private readonly ChessGame _chessGame;
        private MemoryStream _imageStream { get; set; }
        private BitmapImage _bitmapIimage { get; set; }

        public Piece _piece;
        public Piece Piece { get => _piece; set => SetPiece(value); }
        public BoardPosition BoardPosition { get; private set; }

        public event EventHandler<CellMoveEvent> CellMove;

        public BoardCellControl(BoardPosition boardPosition, ChessGame chessGame)
        {
            InitializeComponent();

            _chessGame = chessGame;
            BoardPosition = boardPosition;
            
            if ((boardPosition.Column % 2 == 0 && boardPosition.Row % 2 == 0) ||
                (boardPosition.Column % 2 == 1 && boardPosition.Row % 2 == 1))
                CellBackGroundPanel.Background = Brushes.SaddleBrown;
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

            byte[] image = _piece.PieceType switch
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
            var data = e.Data.GetData(typeof(BoardCellControl));

            if (data is BoardCellControl cell)
            {
                BoardPosition from = cell.BoardPosition;
                BoardPosition to = this.BoardPosition;
                CellMove?.Invoke(this, new CellMoveEvent(_chessGame.CurrentPlayer, from, to));
            }
        }
    }
}
