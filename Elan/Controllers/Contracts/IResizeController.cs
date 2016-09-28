using System.Drawing;
using Elan.Enums;
using Elan.Models.Implementations.Elements;

namespace Elan.Controllers.Contracts
{
    public interface IResizeController : IController
    {
        bool IsResizing { get; }

        bool CanResize { get; }

        RectangleElement[] Corners { get; }

        void UpdateCornersPos();

        void Start(Point startPoint, CornerPosition cornerPosition);

        void Resize(Point currentPoint);

        void End(Point endPoint);

        CornerPosition HitTestCorner(Point point);
    }
}