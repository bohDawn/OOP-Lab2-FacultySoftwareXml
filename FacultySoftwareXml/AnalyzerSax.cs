using System.Xml;

namespace FacultySoftwareXml
{
    public class AnalyzerSax : IAnalyzerXmlStrategy
    {
        public List<SoftwareItem> Search(string path, SoftwareItem criteria)
        {
            List<SoftwareItem> result = new List<SoftwareItem>();
            SoftwareItem item = null;
            string element = "";

            XmlTextReader xmlReader = new XmlTextReader(path);
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        element = xmlReader.Name;
                        if (element == "SoftwareItem")
                            item = new SoftwareItem();
                        break;
                    case XmlNodeType.Text:
                        if (element == "Annotation")
                            item.Annotation = xmlReader.Value;
                        if (element == "UsageTerms")
                            item.UsageTerms = xmlReader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        if (xmlReader.Name == "SoftwareItem" && IsSuitable(item, criteria))
                            result.Add(item);
                        break;
                }

                if (xmlReader.HasAttributes)
                {
                    while (xmlReader.MoveToNextAttribute())
                    {
                        SetAttribute(item, element, xmlReader.Name, xmlReader.Value);
                    }
                }
            }
            xmlReader.Close();

            return result;
        }

        private void SetAttribute(SoftwareItem item, string element, string name, string value)
        {
            if (element == "SoftwareItem")
            {
                if (name == "Id") item.Id = value;
                if (name == "Name") item.Name = value;
                if (name == "Type") item.Type = value;
                if (name == "Version") item.Version = value;
                if (name == "Architecture") item.Architecture = value;
                if (name == "ReleaseYear") item.ReleaseYear = value;
                if (name == "RequiresAdmin") item.RequiresAdmin = value;
            }
            if (element == "Author")
            {
                if (name == "FullName") item.AuthorName = value;
                if (name == "Faculty") item.Faculty = value;
                if (name == "Chair") item.Chair = value;
            }
            if (element == "UsageTerms" && name == "License")
                item.License = value;
            if (element == "Distribution")
            {
                if (name == "Medium") item.DistributionMedium = value;
                if (name == "Path") item.DistributionPath = value;
            }
        }

        private bool IsSuitable(SoftwareItem item, SoftwareItem criteria)
        {
            if (!string.IsNullOrEmpty(criteria.Name) && !item.HasKeyword(criteria.Name))
                return false;
            if (!string.IsNullOrEmpty(criteria.Type) && item.Type != criteria.Type)
                return false;
            if (!string.IsNullOrEmpty(criteria.Faculty) && item.Faculty != criteria.Faculty)
                return false;
            if (!string.IsNullOrEmpty(criteria.Chair) && item.Chair != criteria.Chair)
                return false;
            if (!string.IsNullOrEmpty(criteria.License) && item.License != criteria.License)
                return false;
            return true;
        }
    }
}
