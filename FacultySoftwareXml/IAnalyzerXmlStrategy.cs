namespace FacultySoftwareXml
{
    // Інтерфейс стратегії аналізу XML
    // criteria - що шукаємо, порожні поля не враховуються
    public interface IAnalyzerXmlStrategy
    {
        List<SoftwareItem> Search(string path, SoftwareItem criteria);
    }
}
