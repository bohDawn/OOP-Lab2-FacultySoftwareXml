namespace FacultySoftwareXml
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = new Window(new MainPage());
            window.Title = "Програмне забезпечення на мережі факультету";
#if WINDOWS
            window.HandlerChanged += OnWindowHandlerChanged;
#endif
            return window;
        }

#if WINDOWS
        private void OnWindowHandlerChanged(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            if (window.Handler == null)
                return;

            Microsoft.UI.Xaml.Window nativeWindow = (Microsoft.UI.Xaml.Window)window.Handler.PlatformView;
            nativeWindow.AppWindow.Closing += OnWindowClosing;
        }

        private async void OnWindowClosing(Microsoft.UI.Windowing.AppWindow sender, Microsoft.UI.Windowing.AppWindowClosingEventArgs args)
        {
            args.Cancel = true;

            bool answer = await Windows[0].Page.DisplayAlert("Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
            if (answer)
                Application.Current.Quit();
        }
#endif
    }
}
