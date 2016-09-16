using Elan.Controllers.Contracts;
using Elan.Models.Base;
using Elan.Models.Contracts;

namespace Elan.Controllers.Implementations
{
    internal class CommentBoxController : RectangleController, ILabelController
    {
        public CommentBoxController(BaseElement element)
            : base(element)
        {
        }

        public void SetLabelPosition()
        {
            var label = ((ILabelElement) Element).Label;
            label.Location = Element.Location;
            label.Size = Element.Size;
        }
    }
}