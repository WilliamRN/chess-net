using ChessNet.Data.Enums;
using ChessNet.Data.Models;
using ChessNet.Data.Structs;
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
        
        public Piece Piece { get; set; }

        public BoardCell(int row, int column, ChessGame chessGame)
        {
            InitializeComponent();

            _row = row;
            _column = column;
            _chessGame = chessGame;
        }

        public void Clear()
        {
            CellImage.Source = null;

            if (_bitmapIimage != null)
                _bitmapIimage.StreamSource.Dispose();
        }

        public void SetPiece(Piece piece)
        {
            Piece = piece;

            byte[] image = Piece.GetTypeEnum() switch
            {
                PieceType.Pawn => Piece.IsWhite ? ChessNet.Resources.Images.W_Pawn : ChessNet.Resources.Images.B_Pawn,
                PieceType.King => Piece.IsWhite ? ChessNet.Resources.Images.W_King : ChessNet.Resources.Images.B_King,
                PieceType.Queen => Piece.IsWhite ? ChessNet.Resources.Images.W_Queen : ChessNet.Resources.Images.B_Queen,
                PieceType.Rook => Piece.IsWhite ? ChessNet.Resources.Images.W_Rook : ChessNet.Resources.Images.B_Rook,
                PieceType.Bishop => Piece.IsWhite ? ChessNet.Resources.Images.W_Bishop : ChessNet.Resources.Images.B_Bishop,
                PieceType.Knight => Piece.IsWhite ? ChessNet.Resources.Images.W_Knight : ChessNet.Resources.Images.B_Knight,
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
            if (e.LeftButton == MouseButtonState.Pressed)
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
                        this.SetPiece(cell.Piece);
                        cell.Clear();
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
