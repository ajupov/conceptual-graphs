using System.Drawing;
using Elan.Models.Base;

namespace Elan.Controllers.Contracts
{
    public interface IController
    {
        BaseElement OwnerElement { get; }

        void DrawSelection(Graphics graphics);

        bool HitTest(Point point);

        bool HitTest(Rectangle rectangle);
    }
}