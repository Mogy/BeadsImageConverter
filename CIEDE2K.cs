using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeadsImageConverter
{
    class CIEDE2K
    {
        private static readonly double KL = 1d;
        private static readonly double KC = 1d;
        private static readonly double KH = 1d;
        private static readonly double Max = Diff(CIELAB.White, CIELAB.Black);

        internal static double Pow2(double a) { return a * a; }

        internal static double Pow7(double a) { return a * a * a * a * a * a * a; }

        internal static double Hypot(double a, double b) { return Math.Sqrt(Pow2(a) + Pow2(b)); }

        internal static double Deg2Rad(double deg) { return deg * Math.PI / 180; }

        internal static double Rad2Deg(double rad) { return rad * 180 / Math.PI; }

        public static double Diff(CIELAB src, CIELAB dst)
        {
            double c1 = Hypot(src.A, src.B);
            double c2 = Hypot(dst.A, dst.B);

            double cp7 = Pow7((c1 + c2) / 2);
            double tf7 = Pow7(25);

            double G = 1 - Math.Sqrt(cp7 / (cp7 + tf7));
            double ap1 = src.A + (src.A / 2d) * G;
            double ap2 = dst.A + (dst.A / 2d) * G;

            double cp1 = Hypot(ap1,src.B);
            double cp2 = Hypot(ap2,dst.B);

            double hp1 = (Rad2Deg(Math.Atan2(src.B, ap1)) + 360d) % 360d;
            double hp2 = (Rad2Deg(Math.Atan2(dst.B, ap2)) + 360d) % 360d;
            double h_bar = Math.Abs(hp1 - hp2);

            double dLp = dst.L - src.L;
            double dCp = cp2 - cp1;
            double dHp;
            if (cp1 * cp2 == 0) dHp = 0;
            else
            {
                if (h_bar <= 180d)
                {
                    dHp = hp2 - hp1;
                }
                else if (hp2 <= hp1)
                {
                    dHp = hp2 - hp1 + 360.0;
                }
                else
                {
                    dHp = hp2 - hp1 - 360.0;
                }
            }
            dHp = 2 * Math.Sqrt(cp1 * cp2) * Math.Sin(Deg2Rad(dHp) / 2);

            double aLp = Pow2((src.L + dst.L) / 2d - 50);
            double aCp = (cp1 + cp2) / 2d;
            double aHp;
            if (cp1 * cp2 == 0) aHp = 0;
            else
            {
                if (h_bar <= 180d)
                {
                    aHp = (hp1 + hp2) / 2;
                }
                else if ((hp1 + hp2) < 360d)
                {
                    aHp = (hp1 + hp2 + 360d) / 2;
                }
                else
                {
                    aHp = (hp1 + hp2 - 360d) / 2;
                }
            }

            double T = 1
                - 0.17 * Math.Cos(Deg2Rad(aHp - 30))
                + 0.24 * Math.Cos(Deg2Rad(aHp * 2))
                + 0.32 * Math.Cos(Deg2Rad(aHp * 3 + 6))
                - 0.2 * Math.Cos(Deg2Rad(aHp * 4 - 63));


            double SL = 1 + ((0.015d * aLp) / Math.Sqrt(20 + aLp));
            double SC = 1 + 0.045d * aCp;
            double SH = 1 + 0.015 * aCp * T;

            double aCp7 = Pow7(aCp);
            double RT = -2d * Math.Sqrt(aCp7 / (aCp7 + tf7)) * Math.Sin(Deg2Rad(60d * Math.Exp(-Pow2((aHp - 275d) / 25d))));

            double deltaLp = dLp / (SL * KL);
            double deltaCp = dCp / (SC * KC);
            double deltaHp = dHp / (SH * KH);

            return Math.Sqrt(Pow2(deltaLp) + Pow2(deltaCp) + Pow2(deltaHp) + RT * deltaCp * deltaHp);
        }

        public static double Difference(Color src, Color dst)
        {
            return Difference(CIELAB.FromColor(src), CIELAB.FromColor(dst));
        }

        public static double Difference(CIELAB src, CIELAB dst)
        {
            return Diff(src, dst) / Max;
        }
    }
}
