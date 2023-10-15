using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Testris.App.Core.Objects;
using Testris.App.Core.Utils;

namespace Testris.App.Core
{
    public class Game
    {
        private GameProperties properties;
        private Part? part = null;
        private int tick;
        private Rectangle? baseRec;
        private List<Block> _blocks;
        private bool playing = true;
        private int score = 0;
        private int[,] next = { };
        private Color nextColor = Colors.White;
        public bool IsGameOver { get; private set; }

        public Game(GameProperties properties)
        {
            this.properties = properties;
            _blocks = new List<Block>();
        }

        public void Init()
        {
            GameSettings.Instance().Width = (int)properties.Canvas.Width;
            GameSettings.Instance().Height = (int)properties.Canvas.Height;
            GameSettings.Instance().InitialColumn = GameFunctions.GetCenterColumn();
            GameSettings.Instance().MaxColumn = GameFunctions.GetMaxColumn();
            GameSettings.Instance().MaxRow = GameFunctions.GetMaxRow();
            properties.SetMaxScore(GameSettings.Instance().Score.MaxScore);
            DrawBaseGraphics();
            properties.Music.Play();
        }

        private void DrawBaseGraphics()
        {
            GameGraphics.DrawGrid(properties.Canvas, GameSettings.Instance().BlockSize);
            GameGraphics.DrawGrid(properties.Next, GameSettings.Instance().BlockSizeNext);
            baseRec = GameGraphics.DrawBase(properties.Canvas);
        }

        public void Update()
        {
            if (playing)
            {
                PartUpdate();
            }
        }

        private void TickPart()
        {
            if (tick > 0 && tick > GameSettings.Instance().MoveConst)
            {
                part!.MoveDown();
                DrawPart();
                tick = 0;
                CheckIntersectPart();    
            }
            tick += GameSettings.Instance().TickSpeed;
        }

        private void CheckIntersectPart()
        {
            //Rect rect = GameFunctions.GetBoundsDimenion(part!.Column - 1, part.Row - 1, part.Distribution);
            if ((part!.Row + 1) + part.Bounds.y2 == GameSettings.Instance().MaxRow || CheckBlocks())
            {
                var (bloco1, bloco2, bloco3, bloco4) = part;
                _blocks.Add(bloco1);
                _blocks.Add(bloco2);
                _blocks.Add(bloco3);
                _blocks.Add(bloco4);

                var rangeRow = GameFunctions.RangeRows(part.Distribution, part.Row).ToArray();
                if (!CheckRowsCompleted(rangeRow)) { 
                    if(part.Row + part.Bounds.y2 <= 0)
                    {
                        SetGameOver();
                    }
                };
                part = null;
            }
        }

        private bool CheckRowsCompleted(int[] rows)
        {
            var blocksInRow = _blocks.Where(x => x.Row >= rows.Min() && x.Row <= rows.Max()).GroupBy(x => x.Row);
            List<int> rowsRemoved = new List<int>();
            if(blocksInRow is not null)
            {
                foreach(var item in blocksInRow)
                {
                    if (item.Count() == GameSettings.Instance().MaxColumn)
                    {
                        foreach(var b in item)
                        {
                            _blocks.Remove(b);
                            properties.Canvas.Children.Remove(b.ToRectangle());
                        }
                        rowsRemoved.Add(item.Key);
                    }
                }

                if (rowsRemoved.Count > 0)
                {
                    rowsRemoved.Sort();
                    foreach (int r in rowsRemoved)
                    {
                        var blocksAltos = _blocks.Where(x => x.Row <= r).ToArray();
                        if (blocksAltos is not null && blocksAltos.Length > 0)
                        {
                            foreach (var block in blocksAltos)
                            {
                                block.UpdateRow(block.Row + 1);
                                Canvas.SetTop(block.ToRectangle(), Canvas.GetTop(block.ToRectangle()) + GameSettings.Instance().BlockSize);
                            }
                        }
                    }

                    score += GameSettings.Instance().ValueSumScore * rowsRemoved.Count;
                    properties.SetScore(score);
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        private bool CheckBlocks()
        {
            foreach(var block in _blocks)
            {
                for (int y = 0; y < part!.Distribution.GetLength(0); y++)
                {
                    for (int x = 0; x < part!.Distribution.GetLength(1); x++)
                    {
                        if (part!.Distribution[y, x] == 1)
                        {
                            if (part.Row + 1 + y == block.Row && part.Column + x == block.Column)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        private void SpawnPart()
        {
            if (next.Length == 0)
                part = GameFunctions.CreateNewPart();
            else
                part = GameFunctions.CreateNewPart(next, nextColor);
            
            var values = GameFunctions.CreateNewDistribution();
            next = values.distribution;
            nextColor = values.color;
            part.SetColumn(GameSettings.Instance().InitialColumn);
            DrawPartNext();
        }

        private void PartUpdate()
        {
            if (part is null)
            {
                SpawnPart();
                part!.UpdateLimits();
                DrawPart(true);
            }
            else
            {
                TickPart();
            }
        }

        private void DrawPart(bool add = false)
        {
            var recs = part!.ToRectangles();
            int position = 0;

            for (int y = 0; y < part.Distribution.GetLength(0); y++)
            {
                for (int x = 0; x < part.Distribution.GetLength(1); x++)
                {
                    if (part.Distribution[y, x] == 1)
                    {
                        GameFunctions.SetCanvasPosition(x, y, part.Row, part.Column, recs[position], GameSettings.Instance().BlockSize);
                        position++;
                    }
                }
            }

            if (add)
            {
                foreach (var rec in recs)
                    properties.Canvas.Children.Add(rec);
            }
        }

        private void DrawPartNext()
        {
            int blockSize = GameSettings.Instance().BlockSizeNext;
            var blocks = new Block[] {
                new Block(blockSize, new System.Windows.Point(1,1), nextColor),
                new Block(blockSize, new System.Windows.Point(1,1), nextColor),
                new Block(blockSize, new System.Windows.Point(1,1), nextColor),
                new Block(blockSize, new System.Windows.Point(1,1), nextColor)
            };
            var recs = blocks.Select((b) => b.ToRectangle()).ToArray();
            int position = 0;

            for (int y = 0; y < next.GetLength(0); y++)
            {
                for (int x = 0; x < next.GetLength(1); x++)
                {
                    if (next[y, x] == 1)
                    {
                        GameFunctions.SetCanvasPosition(x, y, 0, 0, recs[position], blockSize);
                        position++;
                    }
                }
            }


            properties.Next.Children.Clear();
            GameGraphics.DrawGrid(properties.Next, blockSize);
            foreach (var rec in recs)
                properties.Next.Children.Add(rec);
            
        }

        public void KeyDown(Key key)
        {
            if(playing)
                MovePart(key);
        }

        public void KeyUp(Key key)
        {
            switch (key)
            {
                case Key.P:
                    Pause();
                    break;
                case Key.Space:
                    ResetGame();
                    break;
            }
        }

        public void ResetGame()
        {
            if (IsGameOver)
            {
                score = 0;
                properties.SetScore(0);
                foreach(var block in _blocks)
                    properties.Canvas.Children.Remove(block.ToRectangle());
                _blocks.Clear();
                part = null;
                playing = true;
                IsGameOver = false;
                properties.Music.Play();
                properties.GameOverPanel.Hide();
            }
        }

        private void MovePart(Key key)
        {
            if (part is null)
                return;

            bool moved = false;
            bool rotate = false;
            switch (key)
            {
                case Key.Left:
                    moved = part.MoveLeft(_blocks.ToArray());        
                    break;
                case Key.Right:
                    moved = part.MoveRight(GameSettings.Instance().MaxColumn, _blocks.ToArray());
                    break;
                case Key.Up:
                    rotate = part.Rotate(_blocks.ToArray());
                    break;
                case Key.Down:
                    tick += GameSettings.Instance().TickSpeed * 6;
                    break;
            }

            if (moved || rotate)
                DrawPart();
        }

        private void Pause()
        {
            if (playing)
            {
                playing = false;
                properties.Music.Pause();
                properties.PausePanel.Show();
            }
            else
            {
                playing = true;
                properties.Music.Play();
                properties.PausePanel.Hide();
            }
        }

        public void SetGameOver()
        {
            playing = false;
            IsGameOver = true;
            if(GameSettings.Instance().Score.SetScore(score))
                properties.SetMaxScore(score);
            properties.Music.Pause();
            properties.GameOverPanel.Show();
        }

    }
}
