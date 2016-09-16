using System.Drawing;

namespace Elan.Controllers.Contracts
{
    public interface IMoveController : IController
    {
        bool IsMoving { get; }
        bool CanMove { get; }

        void Start(Point startPoint);
        void Move(Point currentPoint);
        void End();
    }
}