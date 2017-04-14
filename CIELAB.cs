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

        public readonly double L;
        public readonly double A;
        public readonly double B;

        private static int[] CreateGammaTable()
        {
            int[] table = new int[256];
            for (int i = 0; i < 256; i++)
            {
                double d = i / 255d;
                table[i] = (int)(((d > 0.04045) ? Math.Pow((d + 0.055) / 1.055, 2.4) : (d / 12.92)) * 100000d);
            }
            return table;
        }

        private static int[] CreateLabTable()
        {
            int[] table = new int[100001];
            for (int i = 0; i < table.Count<int>(); i++)
            {
                double d = i / 100001d;
                table[i] = (int)(((d > 0.008856) ? Math.Pow(d, 1d / 3d) : (7.787 * d + 4d / 29d)) * 1000000d);
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

            return new CIELAB(L / 1000000d, A / 1000000d, B / 1000000d);
        }

        public static double Difference(Color src, Color dst)
        {
            return Difference(FromColor(src), FromColor(dst));
        }

        public static double Difference(CIELAB src, CIELAB dst)
        {
            return Diff(src, dst) / Max;
        }

        public CIELAB(double l, double a, double b)
        {
            this.L = l;
            this.A = a;
            this.B = b;
        }
    }
}
