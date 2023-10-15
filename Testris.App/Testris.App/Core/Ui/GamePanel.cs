using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Testris.App.Core.Ui
{
    public class GamePanel
    {
        private Rectangle _panel;

        private string[] texts;
        private List<Label> labels;

        public GamePanel(Rectangle panel, Canvas canvas, params string[] texts)
        {
            _panel = panel;
            panel.Fill = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
            this.texts = texts;
            labels = new();
            InitLabels(canvas);
        }

        public void InitLabels(Canvas canvas)
        {
            for(int i = 0; i < texts.Length; i++)
            {
                Label label = new Label
                {
                    Content = texts[i],
                    FontSize = 15 - i,
                    Foreground = new SolidColorBrush(Colors.White),
                    Visibility = System.Windows.Visibility.Hidden,
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetTop(label, (_panel.Height / 2) + (15 * i) - 15);
                Canvas.SetLeft(label, (_panel.Width / 2) - (texts[i].Length * 4));
                Canvas.SetZIndex(label, 20);
                canvas.Children.Add(label);
                labels.Add(label);
            }
        }

        public void Show()
        {
            _panel.Visibility = System.Windows.Visibility.Visible;
            labels.ForEach(l => {
                l.Visibility = System.Windows.Visibility.Visible;
            });
        }

        public void Hide()
        {
            _panel.Visibility = System.Windows.Visibility.Hidden;
            labels.ForEach(l => {
                l.Visibility = System.Windows.Visibility.Hidden;
            });
        }

    }
}
