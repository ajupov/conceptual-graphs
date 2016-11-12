using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Elan.Enums;
using Elan.Models.Base;
using Elan.Models.Domain;
using Elan.Models.Implementations.Elements;
using Elan.Models.Implementations.Nodes;

namespace Elan.Models.Implementations.Collections
{
    [Serializable]
    public class ElementCollection : ReadOnlyCollectionBase
    {
        #region Свойства
        public const int MaxIntSize = 100;

        private bool _enabledCalc = true;

        private Point _location = new Point(MaxIntSize, MaxIntSize);

        private bool _needCalc = true;

        private Size _size = new Size(0, 0);
        #endregion

        #region Методы и свойства окна
        internal bool EnabledCalc
        {
            get { return _enabledCalc; }
            set
            {
                _enabledCalc = value;

                if (_enabledCalc)
                {
                    _needCalc = true;
                }
            }
        }

        internal Point WindowLocation
        {
            get
            {
                CalcWindow();
                return _location;
            }
        }

        internal Size WindowSize
        {
            get
            {
                CalcWindow();
                return _size;
            }
        }

        internal void CalcWindow(bool forceCalc)
        {
            if (forceCalc)
            {
                _needCalc = true;
            }
            CalcWindow();
        }

        internal void CalcWindow()
        {
            if (!_enabledCalc)
            {
                return;
            }

            if (!_needCalc)
            {
                return;
            }

            _location.X = MaxIntSize;
            _location.Y = MaxIntSize;
            _size.Width = 0;
            _size.Height = 0;
            foreach (BaseElement element in this)
            {
                CalcWindowLocation(element);
            }

            foreach (BaseElement element in this)
            {
                CalcWindowSize(element);
            }

            _needCalc = false;
        }

        internal void CalcWindowLocation(BaseElement element)
        {
            if (!_enabledCalc)
            {
                return;
            }

            var elementLocation = element.Location;

            if (elementLocation.X < _location.X)
            {
                _location.X = elementLocation.X;
            }

            if (elementLocation.Y < _location.Y)
            {
                _location.Y = elementLocation.Y;
            }
        }

        internal void CalcWindowSize(BaseElement element)
        {
            if (!_enabledCalc)
            {
                return;
            }

            var elementLocation = element.Location;
            var elementSize = element.Size;

            var val = elementLocation.X + elementSize.Width - _location.X;
            if (val > _size.Width)
            {
                _size.Width = val;
            }

            val = elementLocation.Y + elementSize.Height - _location.Y;
            if (val > _size.Height)
            {
                _size.Height = val;
            }
        }
        #endregion

        #region Свойства коллекции
        public BaseElement[] GetArray()
        {
            var els = new BaseElement[InnerList.Count];
            for (var i = 0; i <= InnerList.Count - 1; i++)
            {
                els[i] = (BaseElement)InnerList[i];
            }
            return els;
        }

        public List<BaseElement> GetList()
        {
            return new List<BaseElement>(GetArray());
        }

        internal ElementCollection()
        {
        }

        public BaseElement this[int item] => (BaseElement) InnerList[item];

        internal virtual int Add(BaseElement element)
        {
            _needCalc = true;
            return InnerList.Add(element);
        }

        public bool Contains(BaseElement element)
        {
            return InnerList.Contains(element);
        }

        public int IndexOf(BaseElement element)
        {
            return InnerList.IndexOf(element);
        }

        internal void Insert(int index, BaseElement element)
        {
            _needCalc = true;
            InnerList.Insert(index, element);
        }

        internal void Remove(BaseElement element)
        {
            InnerList.Remove(element);
            _needCalc = true;
        }

        internal void Clear()
        {
            InnerList.Clear();
            _needCalc = true;
        }

        internal void ChangeIndex(int i, int y)
        {
            var tmp = InnerList[y];
            InnerList[y] = InnerList[i];
            InnerList[i] = tmp;
        }
        #endregion

        #region Для БД
        public List<Node> GetNodes()
        {
            return GetArray().Select(GetNode).Where(n => n != null).ToList();
        }

        private static Node GetNode(BaseElement element)
        {
            var node = new Node
            {
                Id = element.Id,
                X = element.Location.X,
                Y = element.Location.Y,
                Width = element.Size.Width,
                Height = element.Size.Height
            };

            if (element is RectangleNode)
            {
                var rectangleNode = (RectangleNode)element;
                node.Type = NodeType.Concept;
                node.Label = rectangleNode.Label.Text;
            }
            else if (element is EllipseNode)
            {
                var ellipseNode = (EllipseNode)element;
                node.Type = NodeType.Relation;
                node.Label = ellipseNode.Label.Text;
            }
            else if (element is CommentBoxElement)
            {
                var commentBox = (CommentBoxElement)element;
                node.Type = NodeType.Comment;
                node.Label = commentBox.Label.Text;
            }
            else
            {
                node = null;
            }
            return node;
        }

        public List<Link> GetLinks()
        {
            return GetArray().Select(GetLink).Where(n => n != null).ToList();
        }

        private static Link GetLink(BaseElement element)
        {
            if (element is StraightLinkElement)
            {
                var straightLink = (StraightLinkElement)element;
                return new Link
                {
                    Id = straightLink.Id,
                    Label = straightLink.Label.Text,
                    StartPointX = straightLink.Point1.X,
                    StartPointY = straightLink.Point1.Y,
                    EndPointX = straightLink.Point2.X,
                    EndPointY = straightLink.Point2.Y,
                    StartNodeId = straightLink.Connector1.ParentElement.Id,
                    EndNodeId = straightLink.Connector2.ParentElement.Id
                };
            }
            return null;
        }
        #endregion
    }
}