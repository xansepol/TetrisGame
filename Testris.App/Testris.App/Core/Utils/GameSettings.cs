using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testris.App.Core.Utils
{
    public class GameSettings
    {
        public int Frames { get; private set; } = 30;
        public int BlockSize { get; private set; } = 25;
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;
        public int InitialColumn { get; set; }
        public int MaxColumn { get; set; }
        public int MaxRow { get; set; }
        public int TickSpeed { get; set; } = 75;
        public int MoveConst { get; set; } = 400;
        public int ValueSumScore { get; set; } = 50;
        public int BlockSizeNext { get; set; } = 35;
        public Score Score { get; private set; }

        private static GameSettings? _instance;

        private GameSettings() {
            Score = new Score();
        }

        public static GameSettings Instance()
        {
            if (_instance is null)
                _instance = new GameSettings();

            return _instance;
        }        

    }
}
