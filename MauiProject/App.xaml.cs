namespace MauiProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Вказуємо головну сторінку
            MainPage = new AppShell(); ;
        }
    }
}