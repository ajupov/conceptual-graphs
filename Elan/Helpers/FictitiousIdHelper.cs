using System.Linq;
using Elan.Models.Implementations.Containers;

namespace Elan.Helpers
{
    public static class FictitiousIdHelper
    {
        private static long _currentId;

        public static long NextId => ++_currentId;

        public static void SetCurrentId(this Document document)
        {
            _currentId = document?.Elements?.GetArray().Max(s => s.Id) ?? 0;
        }
    }
}