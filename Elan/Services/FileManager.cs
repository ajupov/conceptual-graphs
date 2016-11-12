using Elan.Models.Domain;
using Elan.Enums;

namespace Elan.Services
{
    public static class FileManager
    {
        public static Document Open(string fileName, FileExtensionType type)
        {
            var document = new Document();

            switch (type)
            {
                case FileExtensionType.Json:
                    document = JsonManager.Open(fileName);
                    break;
                case FileExtensionType.Cgx:
                case FileExtensionType.Xml:
                case FileExtensionType.Txt:
                    document = XmlManager.Open(fileName);
                    break;
            }
            return document;
        }

        public static void Save(string fileName, Document document, FileExtensionType type)
        {
            switch (type)
            {
                case FileExtensionType.Json:
                    JsonManager.Save(fileName, document);
                    break;
                case FileExtensionType.Cgx:
                case FileExtensionType.Xml:
                case FileExtensionType.Txt:
                    XmlManager.Save(fileName, document);
                    break;
            }
        }
    }
}