using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Testris.App.Core;
using Testris.App.Core.Ui;
using Testris.App.Core.Utils;

namespace Testris.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private Game game;
        public MainWindow()
        {
            InitializeComponent();

            game = new Game(new GameProperties(
                    canvas,
                    new GamePanel(pausePanel, canvas, "PAUSE", "PRESS 'P' TO RESUME"),
                    score,
                    max_score,
                    new GamePanel(pausePanel, canvas, "GAME OVER", "PRESS 'SPACE' TO RESET"),
                    nextBlock, 
                    new GameMusic(musicBackground)
                ));

            timer = new DispatcherTimer();

            Init();
        }

        private void Init()
        {
            //Uri iconUri = new Uri("icon.ico", UriKind.RelativeOrAbsolute);
            //this.Icon = BitmapFrame.Create(iconUri);

            game.Init();
            timer.Interval = TimeSpan.FromMilliseconds(1000 / GameSettings.Instance().Frames);
            timer.Tick += IntervalGameHandler;
            timer.Start();
            canvas.Focus();
        }

        private void IntervalGameHandler(object? sender, EventArgs e)
        {
            game.Update();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            game.KeyDown(e.Key);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            game.KeyUp(e.Key);
        }
    }
}
