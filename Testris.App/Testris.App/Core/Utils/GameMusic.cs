using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Testris.App.Core.Utils
{
    public class GameMusic
    {
        public MediaElement BackgroundMusic { get; private set; }

        public GameMusic(MediaElement backgroundMusic)
        {
            BackgroundMusic = backgroundMusic;
            backgroundMusic.Volume = 0.01;
            BackgroundMusic.MediaEnded += (sender, e) => {
                BackgroundMusic.Position = TimeSpan.Zero;
                Play();
            };
        }

        public void Play() => BackgroundMusic.Play();
        public void Pause() => BackgroundMusic.Pause();
    }
}
