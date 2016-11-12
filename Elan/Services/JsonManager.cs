using System.IO;
using Elan.Models.Domain;
using Newtonsoft.Json;

namespace Elan.Services
{
    public static class JsonManager
    {
        public static Document Open(string fileName)
        {
            var serialized = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<Document>(serialized);
        }

        public static void Save(string fileName, Document document)
        {
            var serialized = JsonConvert.SerializeObject(document);
            File.WriteAllText(fileName, serialized);
        }
    }
}