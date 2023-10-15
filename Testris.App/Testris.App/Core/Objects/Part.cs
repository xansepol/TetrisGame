using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using Testris.App.Core.Utils;

namespace Testris.App.Core.Objects
{
    public class Part
    {
        private Block[] _blocks;

        public int Row { get; private set; } = -4;
        public int Column { get; private set; }

        public int[,] Distribution { get; private set; }
        private (int x1, int x2, int y1, int y2) _limitsParts;

        public Part(int[,] distribution, Color color, int blockSize) {
            Distribution = distribution;
            _blocks = new Block[] {
                new Block(blockSize, new System.Windows.Point(1,1), color),
                new Block(blockSize, new System.Windows.Point(1,1), color),
                new Block(blockSize, new System.Windows.Point(1,1), color),
                new Block(blockSize, new System.Windows.Point(1,1), color)
            };
        }

        public void MoveDown() {
            Row++;
        }

        public bool MoveLeft(Block[] blocks)
        {
            if ((Column + _limitsParts.x1) > 0 && CanMoveLeft(blocks))
            {
                Column--;
                return true;
            }
            else
                return false;
        }

        public bool MoveRight(int maxColumn, Block[] blocks)
        {
            if ((Column + _limitsParts.x2) < maxColumn && CanMoveRight(blocks))
            {
                Column++;
                return true;
            }
            else
                return false;
        }

        private bool CanMoveLeft(Block[] blocks)
        {
            foreach (Block block in blocks)
            {
                for (int y = 0; y < Distribution.GetLength(0); y++)
                {
                    for (int x = 0; x < Distribution.GetLength(1); x++)
                    {
                        if (Distribution[y, x] == 1)
                        {
                            if (Column + x - 1 == block.Column && Row + y == block.Row)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool CanMoveRight(Block[] blocks)
        {
            foreach (Block block in blocks)
            {
                for (int y = 0; y < Distribution.GetLength(0); y++)
                {
                    for (int x = 0; x < Distribution.GetLength(1); x++)
                    {
                        if (Distribution[y, x] == 1)
                        {
                            if (Column + x + 1 == block.Column && Row + y == block.Row)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool Rotate(Block[] blocks)
        {
            var rotated = GameFunctions.RotatePart(Distribution);
            if (!GameFunctions.InBounds(Column, rotated) || !CanRotate(blocks, rotated))
                return false;

            Distribution = rotated;
            UpdateLimits();
            return true;
        }

        public bool CanRotate(Block[] blocks, int[,] dimension)
        {
            foreach(var block in blocks)
            {
                for (int y = 0; y < dimension.GetLength(0); y++)
                {
                    for (int x = 0; x < dimension.GetLength(1); x++)
                    {
                        if (dimension[y, x] == 1)
                        {
                            if (Column + x == block.Column && Row + y == block.Row)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        public void UpdateLimits()
        {
            _limitsParts = GameFunctions.GetPartLimits(Distribution);
        }

        public void SetColumn(int column) => Column = column;

        public Rectangle[] ToRectangles() => _blocks.Select((r) => r.ToRectangle()).ToArray() ;

        public void Deconstruct(out Block bloco1, out Block bloco2, out Block bloco3, out Block bloco4)
        {
            int c = 0;
            for(int y = 0; y < Distribution.GetLength(0); y++)
            {
                for (int x = 0; x < Distribution.GetLength(1); x++)
                {
                    if (Distribution[y, x] == 1)
                    {
                        _blocks[c].UpdatePosition(Row + y, Column + x);
                        c++;
                    }
                }
            }
            bloco1 = _blocks[0];
            bloco2 = _blocks[1];
            bloco3 = _blocks[2];
            bloco4 = _blocks[3];
        }

        public (int x1, int x2, int y1, int y2) Bounds => _limitsParts;
    }
}
