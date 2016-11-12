using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Elan.Enums;
using Elan.Models.Domain;
using Elan.Models.Implementations.CharGer;

namespace Elan.Services
{
    public static class XmlManager
    {
        public static Document Open(string filename)
        {
            var root = XElement.Load(new StreamReader(filename, Encoding.GetEncoding("windows-1251")));
            var graph = new Graph(root.Element("graph"));
            var document = new Document
            {
                Name = graph.Type.Label,
                Id = graph.Id
            };

            foreach (var concept in graph.Concepts)
            {
                document.Nodes.Add(new Node
                {
                    DocumentId = graph.Id,
                    Type = NodeType.Concept,
                    Id = concept.Id,
                    X = (int)concept.Layout.Rectangle.X,
                    Y = (int)concept.Layout.Rectangle.Y,
                    Width = (int)concept.Layout.Rectangle.Width,
                    Height = (int)concept.Layout.Rectangle.Height,
                    Label = concept.Type.Label
                });
            }

            foreach (var relation in graph.Relations)
            {
                document.Nodes.Add(new Node
                {
                    DocumentId = graph.Id,
                    Type = NodeType.Relation,
                    Id = relation.Id,
                    X = (int)relation.Layout.Rectangle.X,
                    Y = (int)relation.Layout.Rectangle.Y,
                    Width = (int)relation.Layout.Rectangle.Width,
                    Height = (int)relation.Layout.Rectangle.Height,
                    Label = relation.Type.Label
                });
            }

            foreach (var arrow in graph.Arrows)
            {
                document.Links.Add(new Link
                {
                    DocumentId = graph.Id,
                    Id = arrow.Id,
                    StartNodeId = arrow.From,
                    EndNodeId = arrow.To,
                    StartPointX = 0,
                    StartPointY = 0,
                    EndPointX = 0,
                    EndPointY = 0,
                    Label = arrow.Type.Label
                });
            }
            
            return document;
        }

        public static void Save(string fileName, Document document)
        {
            var userName = !string.IsNullOrEmpty(WindowsIdentity.GetCurrent().Name)
                ? WindowsIdentity.GetCurrent().Name.Split('\\').LastOrDefault()
                : "User";
            var cultureInfo = CultureInfo.GetCultureInfo("en-US");

            var xmlDocument = new XmlDocument();
            var declarationNode = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(declarationNode);

            var conceptualGraphNode = xmlDocument.CreateElement("conceptualgraph");
            AddAttribute(xmlDocument, conceptualGraphNode, "editor", "CharGer");
            AddAttribute(xmlDocument, conceptualGraphNode, "version", 3.6.ToString("F1", cultureInfo));
            AddAttribute(xmlDocument, conceptualGraphNode, "created", DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));
            AddAttribute(xmlDocument, conceptualGraphNode, "user", userName);
            xmlDocument.AppendChild(conceptualGraphNode);

            AddNode(cultureInfo, xmlDocument, conceptualGraphNode, document.Id, 0, document.Name, 5, 5, 900, 750, ChargerXmlElementType.Graph);

            var xmlNodeList = xmlDocument.GetElementsByTagName("graph");
            var rootElementsNode = xmlNodeList?.Item(0);
            if (rootElementsNode == null)
            {
                return;
            }

            foreach (var node in document.Nodes)
            {
                var type = ChargerXmlElementType.Graph;
                switch (node.Type)
                {
                    case NodeType.Concept:
                        type = ChargerXmlElementType.Concept;
                        break;
                    case NodeType.Relation:
                        type = ChargerXmlElementType.Relation;
                        break;
                }
                AddNode(cultureInfo, xmlDocument, rootElementsNode, node.Id, document.Id, node.Label, node.X, node.Y, node.Width, node.Height, type);
            }

            foreach (var link in document.Links)
            {
                AddArrow(cultureInfo, xmlDocument, rootElementsNode, link.Id, document.Id, link.Label, link.StartNodeId, link.EndNodeId, link.StartPointX, link.StartPointY);
            }
            using (var streamWriter = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write), Encoding.GetEncoding("windows-1251")))
            {
                xmlDocument.Save(streamWriter);
            }
        }

        private static void AddNode(IFormatProvider cultureInfo, XmlDocument xmlDocument, XmlNode rootNode, long id, long owner, string label, double x, double y, double width, double height, ChargerXmlElementType type)
        {
            var nodeName = "";
            var foreground = "";
            var background = "";

            switch (type)
            {
                case ChargerXmlElementType.Graph:
                    nodeName = "graph";
                    foreground = "0,0,175";
                    background = "0,0,175";
                    break;
                case ChargerXmlElementType.Concept:
                    nodeName = "concept";
                    foreground = "255,255,255";
                    background = "0,0,175";
                    break;
                case ChargerXmlElementType.Relation:
                    nodeName = "relation";
                    foreground = "0,0,0";
                    background = "255,231,100";
                    break;
            }

            var node = xmlDocument.CreateElement(nodeName);

            AddAttribute(xmlDocument, node, "id", id.ToString());
            AddAttribute(xmlDocument, node, "owner", owner.ToString());
            rootNode.AppendChild(node);

            var typeNode = xmlDocument.CreateElement("type");
            var labelNode = xmlDocument.CreateElement("label");
            labelNode.InnerText = label;
            typeNode.AppendChild(labelNode);
            node.AppendChild(typeNode);

            var layoutNode = xmlDocument.CreateElement("layout");
            var rectangleNode = xmlDocument.CreateElement("rectangle");
            AddAttribute(xmlDocument, rectangleNode, "x", x.ToString("F1", cultureInfo));
            AddAttribute(xmlDocument, rectangleNode, "y", y.ToString("F1", cultureInfo));
            AddAttribute(xmlDocument, rectangleNode, "width", width.ToString("F1", cultureInfo));
            AddAttribute(xmlDocument, rectangleNode, "height", height.ToString("F1", cultureInfo));
            layoutNode.AppendChild(rectangleNode);
            var colorNode = xmlDocument.CreateElement("color");

            AddAttribute(xmlDocument, colorNode, "foreground", foreground);
            AddAttribute(xmlDocument, colorNode, "background", background);
            layoutNode.AppendChild(colorNode);
            node.AppendChild(layoutNode);
            rootNode.AppendChild(node);
        }

        private static void AddArrow(IFormatProvider cultureInfo, XmlDocument xmlDocument, XmlNode rootNode, long id, long owner, string label, long from, long to, double x, double y)
        {
            var node = xmlDocument.CreateElement("arrow");

            AddAttribute(xmlDocument, node, "id", id.ToString());
            AddAttribute(xmlDocument, node, "owner", owner.ToString());
            AddAttribute(xmlDocument, node, "label", label);
            AddAttribute(xmlDocument, node, "from", from.ToString());
            AddAttribute(xmlDocument, node, "to", to.ToString());
            rootNode.AppendChild(node);

            var layoutNode = xmlDocument.CreateElement("layout");
            var rectangleNode = xmlDocument.CreateElement("rectangle");
            AddAttribute(xmlDocument, rectangleNode, "x", x.ToString("F1", cultureInfo));
            AddAttribute(xmlDocument, rectangleNode, "y", y.ToString("F1", cultureInfo));
            AddAttribute(xmlDocument, rectangleNode, "width", 6.0.ToString("F1", cultureInfo));
            AddAttribute(xmlDocument, rectangleNode, "height", 6.0.ToString("F1", cultureInfo));
            layoutNode.AppendChild(rectangleNode);
            var colorNode = xmlDocument.CreateElement("color");

            AddAttribute(xmlDocument, colorNode, "foreground", "0,0,0");
            AddAttribute(xmlDocument, colorNode, "background", "255,255,255");
            layoutNode.AppendChild(colorNode);
            node.AppendChild(layoutNode);
            rootNode.AppendChild(node);
        }

        private static void AddAttribute(XmlDocument xmlDocument, XmlNode conceptualGraphNode, string attributeKey, string attributeValue)
        {
            var editorAttribute = xmlDocument.CreateAttribute(attributeKey);
            editorAttribute.Value = attributeValue;
            conceptualGraphNode.Attributes?.Append(editorAttribute);
        }
    }
}