using ChessNet.Data.Enums;
using ChessNet.Data.Models;
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
        private MemoryStream _imageStream { get; set; }
        private BitmapImage _bitmapIimage { get; set; }

        public BoardCell()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            CellImage.Source = null;

            if (_bitmapIimage != null)
                _bitmapIimage.StreamSource.Dispose();
        }

        public void SetPiece(Piece piece) => SetPiece(piece.Color, piece.GetTypeEnum());

        public void SetPiece(PieceColor color, PieceType piece)
        {
            bool isWhite = color == PieceColor.White;

            byte[] image = piece switch
            {
                PieceType.Pawn => isWhite ? ChessNet.Resources.Images.W_Pawn : ChessNet.Resources.Images.B_Pawn,
                PieceType.King => isWhite ? ChessNet.Resources.Images.W_King : ChessNet.Resources.Images.B_King,
                PieceType.Queen => isWhite ? ChessNet.Resources.Images.W_Queen : ChessNet.Resources.Images.B_Queen,
                PieceType.Rook => isWhite ? ChessNet.Resources.Images.W_Rook : ChessNet.Resources.Images.B_Rook,
                PieceType.Bishop => isWhite ? ChessNet.Resources.Images.W_Bishop : ChessNet.Resources.Images.B_Bishop,
                PieceType.Knight => isWhite ? ChessNet.Resources.Images.W_Knight : ChessNet.Resources.Images.B_Knight,
                _ => null,
            };

            _imageStream = new(image);
            _bitmapIimage = new BitmapImage();
            _bitmapIimage.BeginInit();
            _bitmapIimage.StreamSource = _imageStream;
            _bitmapIimage.EndInit();

            CellImage.Source = _bitmapIimage;
        }
    }
}
