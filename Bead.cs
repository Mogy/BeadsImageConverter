﻿using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeadsImageConverter
{
    public class Bead
    {
        public readonly int No;
        public readonly string Name;
        public readonly XLColor Color;
        public readonly XLColor FontColor;
        public int Count { get; set; }
        public int Total { get; set; }

        public Bead(int no, string name, byte r, byte g, byte b)
        {
            No = no;
            Name = name;
            Color = XLColor.FromArgb((int)r, (int)g, (int)b);
            byte grey = (byte)(0.299 * (double)r + 0.587 * (double)g + 0.114 * (double)b);
            FontColor = ((grey < 186) ? XLColor.White : XLColor.Black);
        }
    }
}
