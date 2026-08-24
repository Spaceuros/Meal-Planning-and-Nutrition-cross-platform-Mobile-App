namespace SpejsMealPlanner;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var loginPage = Handler?.MauiContext?.Services.GetService<LoginPage>();
        return new Window(new NavigationPage(loginPage));
    }
}