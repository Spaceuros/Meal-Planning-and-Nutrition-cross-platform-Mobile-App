using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Data;

namespace SpejsMealPlanner.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly Database _db;

    [ObservableProperty]
    private string pin = string.Empty;

    [ObservableProperty]
    private string loginButtonText = "Mogu sam sve (Solo)";

    public LoginViewModel(Database db)
    {
        _db = db;
    }

    partial void OnPinChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            LoginButtonText = "Mogu sam sve!";
        }
        else
        {
            LoginButtonText = "Pristupi aplikaciji";
        }
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Pin))
        {
            Preferences.Default.Set("UserRole", "Solo");
            Preferences.Default.Set("UserName", "Solo");
            
            Application.Current!.Windows[0].Page = new AppShell();
            return;
        }

        var korisnik = _db.GetUserByPin(Pin.Trim());
        if (korisnik == null)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Greška", "Pogrešan PIN kod. Pokušaj ponovo.", "OK");
            return;
        }

        Preferences.Default.Set("UserRole", korisnik.Role);
        Preferences.Default.Set("UserName", korisnik.Name);
        Preferences.Default.Set("CooksFor", korisnik.CooksFor);

        Application.Current!.Windows[0].Page = new AppShell();
    }

    [RelayCommand]
    private async Task GoToRegisterAsync()
    {
        var registerVm = new RegisterViewModel(_db);
        await Application.Current!.Windows[0].Page!.Navigation.PushAsync(new RegisterPage(registerVm));
    }
}