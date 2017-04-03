using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeadsImageConverter
{
    public class CIELAB
    {
        private static readonly int[] GammaTable = CreateGammaTable();
        private static readonly int[] LabTable = CreateLabTable();
        public static readonly CIELAB White = FromColor(Color.White);
        public static readonly CIELAB Black = FromColor(Color.Black);
        private static readonly double Max = Diff(White, Black);

        public readonly int L;
        public readonly int A;
        public readonly int B;

        private static int[] CreateGammaTable()
        {
            int[] table = new int[256];
            for (int i = 0; i < 256; i++)
            {
                double value = (double)i / 255.0;
                table[i] = (int)(((value > 0.04045) ? Math.Pow((value + 0.055) / 1.055, 2.4) : (value / 12.92)) * 100000.0);
            }
            return table;
        }

        private static int[] CreateLabTable()
        {
            int[] table = new int[100001];
            for (int i = 0; i < table.Count<int>(); i++)
            {
                double value = (double)i / 100001.0;
                table[i] = (int)(((value > 0.008856) ? Math.Pow(value, 0.33333333333333331) : (7.787 * value + 0.13793103448275862)) * 1000000.0);
            }
            return table;
        }

        private static double Diff(CIELAB src, CIELAB dst)
        {
            double L = (double)(src.L - dst.L);
            double A = (double)(src.A - dst.A);
            double B = (double)(src.B - dst.B);
            return Math.Sqrt(L * L + A * A + B * B);
        }

        public static CIELAB FromRGB(byte r, byte g, byte b)
        {
            return CIELAB.FromColor(Color.FromArgb((int)r, (int)g, (int)b));
        }

        public static CIELAB FromColor(Color color)
        {
            int r = GammaTable[(int)color.R];
            int g = GammaTable[(int)color.G];
            int b = GammaTable[(int)color.B];

            int x = (4124 * r + 3576 * g + 1805 * b) / 9505;
            int y = (2126 * r + 7152 * g + 722 * b) / 10000;
            int z = (193 * r + 1192 * g + 9505 * b) / 10890;

            x = LabTable[x];
            y = LabTable[y];
            z = LabTable[z];

            int L = 116 * y - 16000000;
            int A = 500 * (x - y);
            int B = 200 * (y - z);

            return new CIELAB(L, A, B);
        }

        public static double Difference(CIELAB src, CIELAB dst)
        {
            return Diff(src, dst) / Max;
        }

        public CIELAB(int l, int a, int b)
        {
            this.L = l;
            this.A = a;
            this.B = b;
        }
    }
}
