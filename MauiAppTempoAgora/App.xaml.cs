namespace MauiAppTempoAgora
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();//Inicializa o aplicativo

            MainPage = new AppShell();
        }
    }
}
