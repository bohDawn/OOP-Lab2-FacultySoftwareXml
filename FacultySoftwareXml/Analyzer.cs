using System.Xml;
using System.Xml.Xsl;

namespace FacultySoftwareXml
{
    // Контекст: працює з будь-якою стратегією через інтерфейс
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

        // значення атрибута без повторів, щоб заповнити Picker
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

        // перетворення XML в HTML за допомогою XSL
        public static void Transform(string xmlPath, string xslPath, string htmlPath)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xslPath);
            xslt.Transform(xmlPath, htmlPath);
        }
    }
}
