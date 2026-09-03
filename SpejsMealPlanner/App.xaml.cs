namespace SpejsMealPlanner;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        SpejsMealPlanner.Utilities.LocalizationManager.SetLanguage(Preferences.Default.Get("IsEnglish", false));
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var loginPage = Handler?.MauiContext?.Services.GetService<LoginPage>();
        return new Window(new NavigationPage(loginPage));
    }
}