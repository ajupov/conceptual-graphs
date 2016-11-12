using System.IO;
using Elan.Models.Domain;
using Newtonsoft.Json;

namespace Elan.Services
{
    public static class FileManager
    {
        public static void Save(string fileName, Document document)
        {
            var serialized = JsonConvert.SerializeObject(document);
            File.WriteAllText(fileName, serialized);
        }

        public static Document Open(string fileName)
        {
            var serialized = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<Document>(serialized);
        }
    }
}