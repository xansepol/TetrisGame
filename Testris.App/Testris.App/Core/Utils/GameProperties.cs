using System.Windows.Controls;
using Testris.App.Core.Ui;

namespace Testris.App.Core.Utils
{
    public class GameProperties
    {
        public Canvas Canvas { get; private set; }
        public GamePanel PausePanel { get; private set; }
        public GamePanel GameOverPanel { get; private set; }
        public Label Score { get; private set; }
        public Label MaxScore { get; private set; }
        public Canvas Next { get; private set; }
        public GameMusic Music { get; private set; }

        public GameProperties(Canvas canvas, GamePanel pausePanel, Label score, Label maxScore, GamePanel gameOverPanel, Canvas next, GameMusic music)
        {
            Canvas = canvas;
            PausePanel = pausePanel;
            Score = score;
            MaxScore = maxScore;
            GameOverPanel = gameOverPanel;
            Next = next;
            Music = music;
        }

        public void SetScore(int score)
        {
            Score.Content = score.ToString().PadLeft(10, '0');
        }

        public void SetMaxScore(int score)
        {
            MaxScore.Content = score.ToString().PadLeft(10, '0');
        }

    }
}
