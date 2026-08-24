namespace SpejsMealPlanner;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        PodesiUlogu();
    }

    private void PodesiUlogu()
    {
        string uloga = Preferences.Default.Get("UserRole", "Solo");

        if (uloga == "Consumer")
        {
            if (TabSastojci != null) TabSastojci.IsVisible = false;
            if (TabObroci != null) TabObroci.IsVisible = false;
            if (TabHome != null) TabHome.IsVisible = true;
        }
        else if (uloga == "Chef")
        {
            if (TabHome != null) TabHome.IsVisible = false;
            if (TabObroci != null) TabObroci.IsVisible = true;
            if (TabSastojci != null) TabSastojci.IsVisible = true;
        }
        else
        {
            if (TabHome != null) TabHome.IsVisible = true;
            if (TabSastojci != null) TabSastojci.IsVisible = true;
            if (TabObroci != null) TabObroci.IsVisible = true;
        }
    }
}