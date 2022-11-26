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

        public Piece _piece;
        public Piece Piece { get => _piece; set => SetPiece(value); }
        public BoardPosition BoardPosition { get; private set; }

        public event CellUpdateEventHandler CellUpdate;
        public delegate void CellUpdateEventHandler(object sender, CellUpdateEvent e);

        public event CasltingUpdateEventHandler CasltingUpdate;
        public delegate void CasltingUpdateEventHandler(object sender, CasltingUpdateEvent e);

        public event PlayerMoveEventHandler PlayerMove;
        public delegate void PlayerMoveEventHandler(object sender, PlayerMoveEvent e);

        public BoardCell(BoardPosition boardPosition, ChessGame chessGame)
        {
            InitializeComponent();

            _chessGame = chessGame;
            BoardPosition = boardPosition;
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
            var data = e.Data.GetData(typeof(BoardCell));

            if (data is BoardCell cell)
            {
                BoardPosition from = cell.BoardPosition;
                BoardPosition to = this.BoardPosition;

                try
                {
                    var result = _chessGame.Move(from, to);

                    if (result.IsValid)
                    {
                        if (result.IsCastling)
                            CasltingUpdate.Invoke(this, new(cell.Piece.Color));

                        CellUpdate.Invoke(this, new(from));
                        CellUpdate.Invoke(this, new(to));

                        if (result.IsCapture && result.CapturedPiece.Position != to)
                            CellUpdate.Invoke(this, new(result.CapturedPiece.Position));
                    }

                    PlayerMove.Invoke(this, new(result));
                }
                catch
                {
                    // TODO: Alerts and information.
                }
            }
        }
    }
}
