namespace FacultySoftwareXml
{
    // Один програмний засіб з XML-файлу (елемент SoftwareItem)
    public class SoftwareItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }

        // необов'язкові атрибути, можуть бути порожні
        public string Architecture { get; set; }
        public string ReleaseYear { get; set; }
        public string RequiresAdmin { get; set; }

        public string Annotation { get; set; }
        public string AuthorName { get; set; }
        public string Faculty { get; set; }
        public string Chair { get; set; }
        public string License { get; set; }
        public string UsageTerms { get; set; }
        public string DistributionMedium { get; set; }
        public string DistributionPath { get; set; }

        // чи є ключове слово в назві або анотації (регістр не важливий)
        public bool HasKeyword(string keyword)
        {
            if (Name != null && Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                return true;
            if (Annotation != null && Annotation.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        public override string ToString()
        {
            string text = "Назва: " + Name + " " + Version + " (" + Id + ")\n";
            text += "Вид: " + Type + "\n";

            if (!string.IsNullOrEmpty(Architecture))
                text += "Архітектура: " + Architecture + "\n";
            if (!string.IsNullOrEmpty(ReleaseYear))
                text += "Рік випуску: " + ReleaseYear + "\n";
            if (!string.IsNullOrEmpty(RequiresAdmin))
                text += "Права адміністратора: " + RequiresAdmin + "\n";

            text += "Анотація: " + Annotation + "\n";
            text += "Автор: " + AuthorName + "\n";
            text += "Факультет: " + Faculty + "\n";
            text += "Кафедра: " + Chair + "\n";
            text += "Ліцензія: " + License + "\n";
            text += "Умови використання: " + UsageTerms + "\n";
            text += "Дистрибутив: " + DistributionMedium + ", " + DistributionPath + "\n";
            return text;
        }
    }
}
