using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Elan.Models.Base;

namespace Elan.Models.Implementations.Elements
{
    [Serializable]
    public class LineElement : BaseElement
    {
        public LineElement(int x1, int y1, int x2, int y2)
        {
            _startPoint = new Point(x1, y1);
            _endPoint = new Point(x2, y2);
            borderWidth = 1;
        }

        private Point _startPoint;

        private Point _endPoint;

        private bool _needCalc;

        public virtual Point StartPoint
        {
            get
            {
                Calc();
                return _startPoint;
            }
            set
            {
                _startPoint = value;
                _needCalc = true;
                OnAppearanceChanged(new EventArgs());
            }
        }

        public virtual Point EndPoint
        {
            get
            {
                Calc();
                return _endPoint;
            }
            set
            {
                _endPoint = value;
                _needCalc = true;
                OnAppearanceChanged(new EventArgs());
            }
        }

        public void Calc()
        {
            if (!_needCalc)
            {
                return;
            }
            
            if (_startPoint.X < _endPoint.X)
            {
                location.X = _startPoint.X;
                size.Width = _endPoint.X - _startPoint.X;
            }
            else
            {
                location.X = _endPoint.X;
                size.Width = _startPoint.X - _endPoint.X;
            }

            if (_startPoint.Y < _endPoint.Y)
            {
                location.Y = _startPoint.Y;
                size.Height = _endPoint.Y - _startPoint.Y;
            }
            else
            {
                location.Y = _endPoint.Y;
                size.Height = _startPoint.Y - _endPoint.Y;
            }

            _needCalc = false;
        }

        public override void Draw(Graphics graphics)
        {
            IsInvalidated = false;
            using (var pen = new Pen(Color.Black, borderWidth))
            {
                pen.StartCap = LineCap.Round;
                pen.CustomEndCap = new AdjustableArrowCap(5, 5);
                graphics.DrawLine(pen, _startPoint, _endPoint);
            }
        }
    }
}