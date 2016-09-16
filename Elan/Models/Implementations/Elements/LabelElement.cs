using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Elan.Controllers.Contracts;
using Elan.Controllers.Implementations;
using Elan.Models.Base;

namespace Elan.Models.Implementations.Elements
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class LabelElement : BaseElement, ISerializable, IControllable
    {
        public LabelElement()
             : base(0, 0, 100, 100)
        {
            Alignment = StringAlignment.Center;
            LineAlignment = StringAlignment.Center;
            Trimming = StringTrimming.Character;
            Vertical = false;
            Wrap = true;
        }

        [NonSerialized]
        private RectangleController _controller;

        private bool _autoSize;

        private bool _readOnly;

        private string _text = "";

        private StringAlignment _alignment;

        private StringAlignment _lineAlignment;

        private StringTrimming _trimming;

        private bool _vertical;

        private bool _wrap;

        [NonSerialized]
        private Font _font = new Font(FontFamily.GenericSansSerif, 10);

        [NonSerialized]
        private readonly StringFormat _format = new StringFormat(StringFormatFlags.NoWrap);

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (_autoSize)
                {
                    DoAutoSize();
                }
                OnAppearanceChanged(new EventArgs());
            }
        }

        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                if (_autoSize)
                {
                    DoAutoSize();
                }
                OnAppearanceChanged(new EventArgs());
            }
        }

        public StringAlignment Alignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                _format.Alignment = _alignment;
                if (_autoSize)
                {
                    DoAutoSize();
                }
                OnAppearanceChanged(new EventArgs());
            }
        }

        public StringAlignment LineAlignment
        {
            get { return _lineAlignment; }
            set
            {
                _lineAlignment = value;
                _format.LineAlignment = _lineAlignment;
                if (_autoSize)
                {
                    DoAutoSize();
                }
                OnAppearanceChanged(new EventArgs());
            }
        }
        
        public StringTrimming Trimming
        {
            get { return _trimming; }
            set
            {
                _trimming = value;
                _format.Trimming = _trimming;
                if (_autoSize)
                {
                    DoAutoSize();
                }
                OnAppearanceChanged(new EventArgs());
            }
        }

        public bool Wrap
        {
            get { return _wrap; }
            set
            {
                _wrap = value;
                if (_wrap)
                {
                    _format.FormatFlags &= ~StringFormatFlags.NoWrap;
                }
                else
                {
                    _format.FormatFlags |= StringFormatFlags.NoWrap;
                }

                if (_autoSize)
                {
                    DoAutoSize();
                }
                OnAppearanceChanged(new EventArgs());
            }
        }

        public bool Vertical
        {
            get { return _vertical; }
            set
            {
                _vertical = value;
                if (_vertical)
                {
                    _format.FormatFlags |= StringFormatFlags.DirectionVertical;
                }
                else
                {
                    _format.FormatFlags &= ~StringFormatFlags.DirectionVertical;
                }

                if (_autoSize)
                {
                    DoAutoSize();
                }
                OnAppearanceChanged(new EventArgs());
            }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                OnAppearanceChanged(new EventArgs());
            }
        }

        public virtual bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                _autoSize = value;
                if (_autoSize)
                {
                    DoAutoSize();
                }
                OnAppearanceChanged(new EventArgs());
            }
        }

        public override Size Size
        {
            get { return base.Size; }
            set
            {
                size = value;
                if (_autoSize)
                {
                    DoAutoSize();
                }
                base.Size = size;
            }
        }

        public StringFormat Format => _format;

        public IController GetController()
        {
            return _controller ?? (_controller = new RectangleController(this));
        }

        public void DoAutoSize()
        {
            if (_text.Length == 0)
            {
                return;
            }

            Size sizeTmp;
            using (var bitmap = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    sizeTmp = Size.Round(graphics.MeasureString(_text, _font, size.Width, _format));
                }
            }

            if (size.Height < sizeTmp.Height)
            {
                size.Height = sizeTmp.Height;
            }
        }

        public override void Draw(Graphics graphics)
        {
            var rectangle = GetUnsignedRectangle();

            graphics.FillRectangle(new SolidBrush(Color.Empty), rectangle);
            using (var brush = new SolidBrush(Color.Black))
            {
                graphics.DrawString(_text, _font, brush, rectangle, _format);
            }
        }

        public void PositionBySite(BaseElement site)
        {
            Location = new Point(
                site.Location.X + site.Size.Width / 2 - Size.Width / 2,
                site.Location.Y + site.Size.Height / 2 - Size.Height / 2);
        }

        protected LabelElement(SerializationInfo info, StreamingContext context)
        {
            var thisType = typeof(LabelElement);
            var members = FormatterServices.GetSerializableMembers(thisType, context);
            
            foreach (var member in members)
            {
                if (member.DeclaringType == thisType)
                {
                    continue;
                }
                
                var fieldInfo = (FieldInfo) member;
                fieldInfo.SetValue(this, info.GetValue(fieldInfo.Name, fieldInfo.FieldType));
            }
            
            Text = info.GetString("text");
            Alignment = (StringAlignment) info.GetValue("alignment", typeof(StringAlignment));
            LineAlignment = (StringAlignment) info.GetValue("lineAlignment", typeof(StringAlignment));
            Trimming = (StringTrimming) info.GetValue("trimming", typeof(StringTrimming));
            Wrap = info.GetBoolean("wrap");
            Vertical = info.GetBoolean("vertical");
            ReadOnly = info.GetBoolean("readOnly");
            AutoSize = info.GetBoolean("autoSize");

            var fontConverter = new FontConverter();
            Font = (Font) fontConverter.ConvertFromString(info.GetString("_font"));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("text", _text);
            info.AddValue("alignment", _alignment);
            info.AddValue("lineAlignment", _lineAlignment);
            info.AddValue("trimming", _trimming);
            info.AddValue("wrap", _wrap);
            info.AddValue("vertical", _vertical);
            info.AddValue("readOnly", _readOnly);
            info.AddValue("autoSize", _autoSize);

            var fontConverter = new FontConverter();
            info.AddValue("_font", fontConverter.ConvertToString(_font));
            
            var thisType = typeof(LabelElement);
            var members = FormatterServices.GetSerializableMembers(thisType, context);
            
            foreach (var member in members)
            {
                if (member.DeclaringType == thisType)
                {
                    continue;
                }
                info.AddValue(member.Name, ((FieldInfo) member).GetValue(this));
            }
        }
    }
}