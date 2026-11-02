namespace FacultySoftwareXml
{
    public partial class MainPage : ContentPage
    {
        private string xmlPath = "";
        private string xslPath = "";
        private string htmlPath = Path.Combine(FileSystem.AppDataDirectory, "result.html");

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (xmlPath != "")
                return;

            xmlPath = await CopyFromResources("software_catalog.xml");
            xslPath = await CopyFromResources("transform.xsl");
            XmlFileLabel.Text = xmlPath;
            XslFileLabel.Text = xslPath;
            FillPickers();
        }

        private async void OnOpenXmlClicked(object sender, EventArgs e)
        {
            string path = await PickFile(".xml");
            if (path == null)
                return;

            xmlPath = path;
            XmlFileLabel.Text = path;
            try
            {
                FillPickers();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", "Не вдалося прочитати файл: " + ex.Message, "OK");
            }
        }

        private async void OnOpenXslClicked(object sender, EventArgs e)
        {
            string path = await PickFile(".xsl");
            if (path == null)
                return;

            xslPath = path;
            XslFileLabel.Text = path;
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            try
            {
                Analyzer analyzer = new Analyzer(GetStrategy());
                List<SoftwareItem> result = analyzer.Search(xmlPath, GetCriteria());
                ShowResult(result);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", ex.Message, "OK");
            }
        }

        private async void OnTransformClicked(object sender, EventArgs e)
        {
            try
            {
                Analyzer.Transform(xmlPath, xslPath, htmlPath);
                await Launcher.Default.OpenAsync(new OpenFileRequest("HTML", new ReadOnlyFile(htmlPath)));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", ex.Message, "OK");
            }
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            NameEntry.Text = "";
            TypeCheck.IsChecked = false;
            FacultyCheck.IsChecked = false;
            ChairCheck.IsChecked = false;
            LicenseCheck.IsChecked = false;

            TypePicker.SelectedIndex = -1;
            FacultyPicker.SelectedIndex = -1;
            ChairPicker.SelectedIndex = -1;
            LicensePicker.SelectedIndex = -1;

            ResultLabel.Text = "";
            CountLabel.Text = "Результати пошуку:";
        }

        private async void OnExitClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
            if (answer)
                Application.Current.Quit();
        }

        private IAnalyzerXmlStrategy GetStrategy()
        {
            if (DomRadio.IsChecked)
                return new AnalyzerDom();
            if (LinqRadio.IsChecked)
                return new AnalyzerLinq();
            return new AnalyzerSax();
        }

        private SoftwareItem GetCriteria()
        {
            SoftwareItem criteria = new SoftwareItem();
            criteria.Name = NameEntry.Text;
            criteria.Type = GetValue(TypeCheck, TypePicker);
            criteria.Faculty = GetValue(FacultyCheck, FacultyPicker);
            criteria.Chair = GetValue(ChairCheck, ChairPicker);
            criteria.License = GetValue(LicenseCheck, LicensePicker);
            return criteria;
        }

        private string GetValue(CheckBox check, Picker picker)
        {
            if (check.IsChecked && picker.SelectedItem != null)
                return picker.SelectedItem.ToString();
            return "";
        }

        private void FillPickers()
        {
            TypePicker.ItemsSource = Analyzer.GetValues(xmlPath, "//SoftwareItem/@Type");
            FacultyPicker.ItemsSource = Analyzer.GetValues(xmlPath, "//Author/@Faculty");
            ChairPicker.ItemsSource = Analyzer.GetValues(xmlPath, "//Author/@Chair");
            LicensePicker.ItemsSource = Analyzer.GetValues(xmlPath, "//UsageTerms/@License");
        }

        private void ShowResult(List<SoftwareItem> result)
        {
            CountLabel.Text = "Результати пошуку: знайдено " + result.Count;
            string text = "";
            foreach (SoftwareItem item in result)
            {
                text += item.ToString() + "\n";
            }
            ResultLabel.Text = text;
        }

        private async Task<string> PickFile(string extension)
        {
            PickOptions options = new PickOptions();
            options.FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { extension } }
            });

            FileResult file = await FilePicker.Default.PickAsync(options);
            if (file == null)
                return null;
            return file.FullPath;
        }

        private async Task<string> CopyFromResources(string fileName)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
            using (Stream input = await FileSystem.OpenAppPackageFileAsync(fileName))
            using (FileStream output = File.Create(path))
            {
                await input.CopyToAsync(output);
            }
            return path;
        }
    }
}
