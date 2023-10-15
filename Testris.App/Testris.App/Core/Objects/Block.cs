using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Testris.App.Core.Objects
{
    public class Block
    {

        private Rectangle _rect;
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Block(int blockSize, Point point, Color color) {
            _rect = new()
            {
                Width = blockSize,
                Height = blockSize,
                Fill = new SolidColorBrush(color),
                Stroke = new SolidColorBrush(Color.FromRgb(142,250,250)),
                Tag = "game.block"
            };
        }

        public void UpdatePosition(int row, int column)
        {
            Column = column;
            Row = row;
        }

        public void UpdateRow(int row)
        {
            Row = row;
        }

        public void UpdateColumn(int row, int column)
        {
            Column = column;
            Row = row;
        }

        public Rectangle ToRectangle() => _rect;       

        

    }
}
