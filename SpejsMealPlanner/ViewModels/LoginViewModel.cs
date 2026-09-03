using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Data;
using SpejsMealPlanner.Utilities;

namespace SpejsMealPlanner.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly Database _db;

    [ObservableProperty]
    private string pin = string.Empty;

    [ObservableProperty]
    private string loginButtonText = string.Empty;
    
    [ObservableProperty]
    private bool isDarkMode;
    
    [ObservableProperty]
    private bool isEnglish;

    [ObservableProperty]
    private string themeIcon = string.Empty;
    
    public LoginViewModel(Database db)
    {
        _db = db;
        
        bool isSystemDark = Application.Current!.RequestedTheme == AppTheme.Dark;
        isDarkMode = Preferences.Default.Get("IsDarkMode", isSystemDark);
        
        ThemeIcon = isDarkMode ? "☾" : "☀";
        Application.Current!.UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;
        
        IsEnglish = Preferences.Default.Get("IsEnglish", false);
        LocalizationManager.SetLanguage(IsEnglish);
        UpdateButtonText();
    }

    partial void OnPinChanged(string value)
    {
        UpdateButtonText();
    }
    
    partial void OnIsDarkModeChanged(bool value)
    {
        Application.Current!.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
        ThemeIcon = value ? "☾" : "☀";
        Preferences.Default.Set("IsDarkMode", value);
    }
    
    partial void OnIsEnglishChanged(bool value)
    {
        Preferences.Default.Set("IsEnglish", value);
        LocalizationManager.SetLanguage(value);
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (string.IsNullOrWhiteSpace(Pin))
        {
            LoginButtonText = LocalizationManager.Translate("SoloButtonEmpty", IsEnglish);
        }
        else
        {
            LoginButtonText = LocalizationManager.Translate("SoloButtonFilled", IsEnglish);
        }
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
    }

    [RelayCommand]
    private void ToggleLanguage()
    {
        IsEnglish = !IsEnglish;
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
            string errorTitle = LocalizationManager.Translate("Error", IsEnglish);
            string errorMessage = LocalizationManager.Translate("ErrorWrongPin", IsEnglish);
            
            await Application.Current!.Windows[0].Page!.DisplayAlert(errorTitle, errorMessage, LocalizationManager.Translate("Ok", IsEnglish));
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