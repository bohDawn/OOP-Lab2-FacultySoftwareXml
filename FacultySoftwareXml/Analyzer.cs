using System.Xml;
using System.Xml.Xsl;

namespace FacultySoftwareXml
{
    public class Analyzer
    {
        private IAnalyzerXmlStrategy strategy;

        public Analyzer(IAnalyzerXmlStrategy strategy)
        {
            this.strategy = strategy;
        }

        public List<SoftwareItem> Search(string path, SoftwareItem criteria)
        {
            return strategy.Search(path, criteria);
        }

        public static List<string> GetValues(string path, string xpath)
        {
            List<string> values = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            foreach (XmlNode node in xmlDoc.SelectNodes(xpath))
            {
                if (!values.Contains(node.Value))
                    values.Add(node.Value);
            }
            values.Sort();
            return values;
        }

        public static void Transform(string xmlPath, string xslPath, string htmlPath)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xslPath);
            xslt.Transform(xmlPath, htmlPath);
        }

        public static void SaveToXml(List<SoftwareItem> items, string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement root = xmlDoc.CreateElement("FacultySoftwareCatalog");
            xmlDoc.AppendChild(root);

            foreach (SoftwareItem item in items)
            {
                XmlElement element = xmlDoc.CreateElement("SoftwareItem");
                AddAttribute(xmlDoc, element, "Id", item.Id);
                AddAttribute(xmlDoc, element, "Name", item.Name);
                AddAttribute(xmlDoc, element, "Type", item.Type);
                AddAttribute(xmlDoc, element, "Version", item.Version);
                AddAttribute(xmlDoc, element, "Architecture", item.Architecture);
                AddAttribute(xmlDoc, element, "ReleaseYear", item.ReleaseYear);
                AddAttribute(xmlDoc, element, "RequiresAdmin", item.RequiresAdmin);
                root.AppendChild(element);

                XmlElement annotation = xmlDoc.CreateElement("Annotation");
                annotation.InnerText = item.Annotation;
                element.AppendChild(annotation);

                XmlElement author = xmlDoc.CreateElement("Author");
                AddAttribute(xmlDoc, author, "FullName", item.AuthorName);
                AddAttribute(xmlDoc, author, "Faculty", item.Faculty);
                AddAttribute(xmlDoc, author, "Chair", item.Chair);
                element.AppendChild(author);

                XmlElement terms = xmlDoc.CreateElement("UsageTerms");
                AddAttribute(xmlDoc, terms, "License", item.License);
                terms.InnerText = item.UsageTerms;
                element.AppendChild(terms);

                XmlElement distribution = xmlDoc.CreateElement("Distribution");
                AddAttribute(xmlDoc, distribution, "Medium", item.DistributionMedium);
                AddAttribute(xmlDoc, distribution, "Path", item.DistributionPath);
                element.AppendChild(distribution);
            }

            xmlDoc.Save(path);
        }

        private static void AddAttribute(XmlDocument xmlDoc, XmlElement element, string name, string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            XmlAttribute attr = xmlDoc.CreateAttribute(name);
            attr.Value = value;
            element.Attributes.Append(attr);
        }
    }
}
