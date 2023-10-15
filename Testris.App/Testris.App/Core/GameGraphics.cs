using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Testris.App.Core
{
    public class GameGraphics
    {
        public static void DrawGrid(Canvas canvas, int blockSize)
        {
            int columns = (int)Math.Round((double)canvas.Width / blockSize);
            int rows = (int)Math.Round((double)canvas.Height / blockSize);

            for(int i = 1;i <= columns; i++)
            {
                var x = i * blockSize;
                Line line = new(){
                    X1 = x,
                    X2 = x,
                    Y1 = 1,
                    Y2 = canvas.Height,
                    Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    
                };
                Canvas.SetZIndex(line, 0);
                canvas.Children.Add(line);
            }

            for (int i = 1; i <= rows; i++)
            {
                var y = i * blockSize;
                Line line = new()
                {
                    X1 = 1,
                    X2 = canvas.Width,
                    Y1 = y,
                    Y2 = y,
                    Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255)),

                };
                Canvas.SetZIndex(line, 0);
                canvas.Children.Add(line);
            }
        }

        public static Rectangle DrawBase(Canvas canvas)
        {
            Rectangle rectangle = new()
            {
                Width = canvas.Width,
                Height = 25,
                Fill = new SolidColorBrush(Color.FromRgb(0, 0, 100)),
                Tag = "Canvas.base"
            };
            Canvas.SetZIndex(rectangle, 1);
            Canvas.SetLeft(rectangle, 0);
            Canvas.SetTop(rectangle, canvas.Height - 50);
            canvas.Children.Add(rectangle);
            return rectangle;
        }

    }
}
