using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Testris.App.Core.Utils.Extensions
{
    public static class RectangleExtensions
    {
        public static Rect ToRect(this Rectangle rectangle) =>
            new Rect(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle), rectangle.Width, rectangle.Height);
    }
}
