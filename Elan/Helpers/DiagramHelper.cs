using System;
using System.Drawing;
using Elan.Enums;

namespace Elan.Helpers
{
    internal static class DiagramHelper
    {
        public static CardinalDirection GetDirection(Rectangle rectangle, Point point)
        {
            var cartesianCoordinate = DisplayToCartesianCoordinate(point, rectangle);
            var angle = PointToAngle(cartesianCoordinate);

            //East
            if (((angle >= 0) && (angle < 45)) || (angle >= 315))
            {
                return CardinalDirection.East;
            }
            //North
            if ((angle >= 45) && (angle < 135))
            {
                return CardinalDirection.North;
            }
            //West
            if ((angle >= 135) && (angle < 225))
            {
                return CardinalDirection.West;
            }
            //South
            if ((angle >= 225) && (angle < 315))
            {
                return CardinalDirection.South;
            }

            return CardinalDirection.Nothing;
        }
        public static Point GetUpperPoint(Point[] points)
        {
            var upper = new Point
            {
                X = int.MaxValue,
                Y = int.MaxValue
            };

            foreach (var point in points)
            {
                if (point.X < upper.X)
                {
                    upper.X = point.X;
                }

                if (point.Y < upper.Y)
                {
                    upper.Y = point.Y;
                }
            }

            return upper;
        }
        public static Point GetLowerPoint(Point[] points)
        {
            var lower = new Point
            {
                X = int.MinValue,
                Y = int.MinValue
            };

            foreach (var point in points)
            {
                if (point.X > lower.X)
                {
                    lower.X = point.X;
                }

                if (point.Y > lower.Y)
                {
                    lower.Y = point.Y;
                }
            }

            return lower;
        }
        public static Size MeasureString(string text, Font font)
        {
            SizeF size;
            using (var bmp = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bmp))
                {
                    size = graphics.MeasureString(text, font);
                }
            }
            return Size.Round(size);
        }
        public static Size MeasureString(string text, Font font, int width, StringFormat format)
        {
            SizeF size;
            using (var bmp = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bmp))
                {
                    size = graphics.MeasureString(text, font, width, format);
                }
            }
            return Size.Round(size);
        }
        private static Point DisplayToCartesianCoordinate(Point point, Rectangle referenceRec)
        {
            return new Point(point.X - referenceRec.Width / 2, point.Y - referenceRec.Height / 2);
        }
        private static double PointToAngle(Point point)
        {
            var angle = Math.Atan2(point.Y, point.X) * (180 / Math.PI);

            if ((angle > 0) && (angle < 180))
            {
                angle = 360 - angle;
            }
            angle = Math.Abs(angle);

            return angle;
        }
    }
}