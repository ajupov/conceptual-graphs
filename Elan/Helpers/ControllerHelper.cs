using Elan.Controllers.Contracts;
using Elan.Models.Base;
using Elan.Models.Contracts;

namespace Elan.Helpers
{
    public static class ControllerHelper
    {
        public static IMoveController GetMoveController(BaseElement element)
        {
            var controllable = element as IControllable;
            return controllable?.GetController() as IMoveController;
        }

        public static ILabelController GetLabelController(BaseElement element)
        {
            var controllable = element as IControllable;
            return controllable != null && element is ILabelElement
                ? controllable.GetController() as ILabelController
                : null;
        }
    }
}