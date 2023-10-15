using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Testris.App.Core.Objects;

namespace Testris.App.Core.Utils
{
    public static class GameFunctions
    {
        private static int[][,] distributuions = new int[7][,]{
            new int[4,4]{ 
                {0, 0, 0, 0},
                {1, 1, 1, 1},
                {0, 0, 0, 0},
                {0, 0, 0, 0} 
            },
            new int[4,4]{
                {0, 0, 0, 0},
                {1, 1, 0, 0},
                {0, 1, 1, 0},
                {0, 0, 0, 0}
            },
            new int[4,4]{
                {0, 0, 0, 0},
                {0, 1, 1, 0},
                {1, 1, 0, 0},
                {0, 0, 0, 0}
            },
            new int[4,4]{
                {0, 0, 0, 0},
                {0, 1, 1, 0},
                {0, 1, 1, 0},
                {0, 0, 0, 0}
            },
            new int[4,4]{
                {0, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 1, 0, 0},
                {0, 1, 1, 0}
            },
            new int[4,4]{
                {0, 0, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 1, 0},
                {0, 1, 1, 0}
            },
            new int[4,4]{
                {0, 0, 0, 0},
                {0, 1, 0, 0},
                {1, 1, 1, 0},
                {0, 0, 0, 0}
            }
        };

        private static Color[] _colors = {
            Color.FromRgb(221, 22, 35),
            Color.FromRgb(247, 93, 4),
            Color.FromRgb(22, 105, 221),
            Color.FromRgb(255, 242, 0),
            Color.FromRgb(51, 209, 46),
            Color.FromRgb(228, 95, 252),
            Color.FromRgb(92, 84, 112)
        };

        public static Part CreateNewPart()
        {
            int index = Random.Shared.Next(0, distributuions.Length);
            var distribution = distributuions[index];
            var color = _colors[index];
            return new Part(distribution, color, GameSettings.Instance().BlockSize);
        }

        public static (int[,] distribution, Color color) CreateNewDistribution()
        {
            int index = Random.Shared.Next(0, distributuions.Length);
            var distribution = distributuions[index];
            var color = _colors[index];
            return (distribution, color);
        }

        public static Part CreateNewPart(int[,] distribution, Color color)
        {
            return new Part(distribution, color, GameSettings.Instance().BlockSize);
        }

        public static int GetCenterColumn()
        {
            return ((int)(GameSettings.Instance().Width / GameSettings.Instance().BlockSize) / 2) - 2;
        }

        public static int GetMaxColumn()
        {
            return (int)(GameSettings.Instance().Width / GameSettings.Instance().BlockSize);
        }

        public static int GetMaxRow()
        {
            return (int)(GameSettings.Instance().Height / GameSettings.Instance().BlockSize) - 1;
        }

        public static void SetCanvasPosition(int x, int y, int row, int column, Rectangle rectangle, int blockSize)
        {
            var left = (column * blockSize) + (x * blockSize);
            var top = (row * blockSize) + (y * blockSize);
            Canvas.SetLeft(rectangle, left);
            Canvas.SetTop(rectangle, top);
        }

        public  static (int x1, int x2, int y1, int y2) GetPartLimits(int[,] dimension)
        {
            int valueX = 5;
            int valueY = 5;
            int valueX2 = 0;
            int valueY2 = 0;
            for (int y = 0; y < dimension.GetLength(0); y++)
            {
                for (int x = 0; x < dimension.GetLength(1); x++)
                {
                    if (dimension[y, x] == 1)
                    {
                        if (x > valueX2)
                            valueX2 = x;
                        if (x < valueX)
                            valueX = x;
                        if (y > valueY2)
                            valueY2 = y;
                        if (y < valueY)
                            valueY = y;
                    }
                }
            }
            return (valueX, valueX2 + 1, valueY, valueY2 + 1);
        }

        public static int[,] RotatePart(int[,] mat)
        {
            int[,] matrizRotacionada = new int[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    matrizRotacionada[i, j] = mat[3 - j, i];
            }

            return matrizRotacionada;
        }

        public static bool InBounds(int column, int[,] dimension)
        {
            var limits = GetPartLimits(dimension);
            return (column + limits.x1) >= 0 && (column + limits.x2) <= GameSettings.Instance().MaxColumn;
        }

        public static Rect GetBoundsDimenion(int column, int row, int[,] dimension)
        {
            var limits = GetPartLimits(dimension);
            var sizeBlock = GameSettings.Instance().BlockSize;
            return new Rect(
                (column + limits.x1) * sizeBlock,
                (row + limits.y1) * sizeBlock,
                ((column + limits.x1 + limits.x2) - (column + limits.x1)) * sizeBlock,
                ((row + limits.y1 + limits.y2) - (row + limits.y1)) * sizeBlock
            );
        }

        public static IEnumerable<int> RangeRows(int[,] dimension, int row)
        {
            for (int i = 0; i < dimension.GetLength(0); i++)
                yield return row + i;

            yield break;
        }
    }
}
