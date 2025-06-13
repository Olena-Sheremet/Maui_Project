namespace MauiProject
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            // Вказуємо головну сторінку
            MainPage = new NavigationPage(mainPage);
        }
    }
}