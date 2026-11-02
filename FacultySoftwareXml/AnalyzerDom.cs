using System.Xml;

namespace FacultySoftwareXml
{
    public class AnalyzerDom : IAnalyzerXmlStrategy
    {
        public List<SoftwareItem> Search(string path, SoftwareItem criteria)
        {
            List<SoftwareItem> result = new List<SoftwareItem>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            XmlNodeList nodes = xmlDoc.SelectNodes(MakeXPath(criteria));
            foreach (XmlNode node in nodes)
            {
                SoftwareItem item = ReadItem(node);
                if (string.IsNullOrEmpty(criteria.Name) || item.HasKeyword(criteria.Name))
                    result.Add(item);
            }
            return result;
        }

        private string MakeXPath(SoftwareItem criteria)
        {
            string conditions = "";

            if (!string.IsNullOrEmpty(criteria.Type))
                conditions = AddCondition(conditions, "@Type=\"" + criteria.Type + "\"");
            if (!string.IsNullOrEmpty(criteria.Faculty))
                conditions = AddCondition(conditions, "Author/@Faculty=\"" + criteria.Faculty + "\"");
            if (!string.IsNullOrEmpty(criteria.Chair))
                conditions = AddCondition(conditions, "Author/@Chair=\"" + criteria.Chair + "\"");
            if (!string.IsNullOrEmpty(criteria.License))
                conditions = AddCondition(conditions, "UsageTerms/@License=\"" + criteria.License + "\"");

            if (conditions == "")
                return "//SoftwareItem";
            return "//SoftwareItem[" + conditions + "]";
        }

        private string AddCondition(string conditions, string condition)
        {
            if (conditions == "")
                return condition;
            return conditions + " and " + condition;
        }

        private SoftwareItem ReadItem(XmlNode node)
        {
            SoftwareItem item = new SoftwareItem();

            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name == "Id") item.Id = attr.Value;
                if (attr.Name == "Name") item.Name = attr.Value;
                if (attr.Name == "Type") item.Type = attr.Value;
                if (attr.Name == "Version") item.Version = attr.Value;
                if (attr.Name == "Architecture") item.Architecture = attr.Value;
                if (attr.Name == "ReleaseYear") item.ReleaseYear = attr.Value;
                if (attr.Name == "RequiresAdmin") item.RequiresAdmin = attr.Value;
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "Annotation")
                    item.Annotation = child.InnerText;
                if (child.Name == "Author")
                {
                    item.AuthorName = child.Attributes["FullName"].Value;
                    item.Faculty = child.Attributes["Faculty"].Value;
                    item.Chair = child.Attributes["Chair"].Value;
                }
                if (child.Name == "UsageTerms")
                {
                    item.License = child.Attributes["License"].Value;
                    item.UsageTerms = child.InnerText;
                }
                if (child.Name == "Distribution")
                {
                    item.DistributionMedium = child.Attributes["Medium"].Value;
                    item.DistributionPath = child.Attributes["Path"].Value;
                }
            }
            return item;
        }
    }
}
