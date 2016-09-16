using Elan.Models.Implementations.Collections;

namespace Elan.Models.Base
{
    public interface IContainer
    {
        ElementCollection Elements { get; }
    }
}