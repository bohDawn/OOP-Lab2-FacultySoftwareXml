namespace FacultySoftwareXml
{
    public interface IAnalyzerXmlStrategy
    {
        List<SoftwareItem> Search(string path, SoftwareItem criteria);
    }
}
