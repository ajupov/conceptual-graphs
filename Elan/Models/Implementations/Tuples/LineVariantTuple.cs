using System;
using System.Drawing;

namespace Elan.Models.Implementations.Tuples
{
    public class LineVariantTuple
    {
        public Point StartPoint { get; set; }
        
        public Point EndPoint { get; set; }

        public double Length => Math.Sqrt(Math.Pow(EndPoint.X - StartPoint.X, 2)
            + Math.Pow(EndPoint.X - StartPoint.X, 2));
    }
}