using System.Xml.Linq;

namespace FacultySoftwareXml
{
    public class AnalyzerLinq : IAnalyzerXmlStrategy
    {
        public List<SoftwareItem> Search(string path, SoftwareItem criteria)
        {
            var doc = XDocument.Load(path);

            var result = (from item in doc.Descendants("SoftwareItem")
                          where (string.IsNullOrEmpty(criteria.Name)
                                 || ((string)item.Attribute("Name") ?? "").Contains(criteria.Name, StringComparison.OrdinalIgnoreCase)
                                 || ((string)item.Element("Annotation") ?? "").Contains(criteria.Name, StringComparison.OrdinalIgnoreCase))
                             && (string.IsNullOrEmpty(criteria.Type) || (string)item.Attribute("Type") == criteria.Type)
                             && (string.IsNullOrEmpty(criteria.Faculty) || (string)item.Element("Author").Attribute("Faculty") == criteria.Faculty)
                             && (string.IsNullOrEmpty(criteria.Chair) || (string)item.Element("Author").Attribute("Chair") == criteria.Chair)
                             && (string.IsNullOrEmpty(criteria.License) || (string)item.Element("UsageTerms").Attribute("License") == criteria.License)
                          select new SoftwareItem
                          {
                              Id = (string)item.Attribute("Id"),
                              Name = (string)item.Attribute("Name"),
                              Type = (string)item.Attribute("Type"),
                              Version = (string)item.Attribute("Version"),
                              Architecture = (string)item.Attribute("Architecture"),
                              ReleaseYear = (string)item.Attribute("ReleaseYear"),
                              RequiresAdmin = (string)item.Attribute("RequiresAdmin"),
                              Annotation = (string)item.Element("Annotation"),
                              AuthorName = (string)item.Element("Author").Attribute("FullName"),
                              Faculty = (string)item.Element("Author").Attribute("Faculty"),
                              Chair = (string)item.Element("Author").Attribute("Chair"),
                              License = (string)item.Element("UsageTerms").Attribute("License"),
                              UsageTerms = (string)item.Element("UsageTerms"),
                              DistributionMedium = (string)item.Element("Distribution").Attribute("Medium"),
                              DistributionPath = (string)item.Element("Distribution").Attribute("Path")
                          }).ToList();

            return result;
        }
    }
}
