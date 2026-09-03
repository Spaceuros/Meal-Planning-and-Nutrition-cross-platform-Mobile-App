using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Data;
using SpejsMealPlanner.Models;
using SpejsMealPlanner.Utilities;
using System.Collections.ObjectModel;

namespace SpejsMealPlanner.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly Database _db;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string pin = string.Empty;

    [ObservableProperty]
    private User? selectedConsumer;

    [ObservableProperty]
    private bool isChef;

    [ObservableProperty]
    private bool isConsumer = true;

    [ObservableProperty]
    private string consumerButtonColor = "#0066cc";

    [ObservableProperty]
    private string chefButtonColor = "Transparent";

    public ObservableCollection<User> AvailableConsumers { get; set; } = new();

    public RegisterViewModel(Database db)
    {
        _db = db;
        UcitajKonzumente();
    }

    private void UcitajKonzumente()
    {
        AvailableConsumers.Clear();
        var lista = _db.GetConsumers();
        foreach (var c in lista)
        {
            AvailableConsumers.Add(c);
        }
    }

    [RelayCommand]
    private void SelectConsumer()
    {
        IsConsumer = true;
        IsChef = false;
        ConsumerButtonColor = "#0066cc";
        ChefButtonColor = "Transparent";
    }

    [RelayCommand]
    private void SelectChef()
    {
        IsConsumer = false;
        IsChef = true;
        ConsumerButtonColor = "Transparent";
        ChefButtonColor = "#0066cc";
        UcitajKonzumente();
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        var page = Application.Current!.Windows[0].Page!;
        await page.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        var page = Application.Current!.Windows[0].Page!;
        bool isEnglish = Preferences.Default.Get("IsEnglish", false);
        string errorTitle = LocalizationManager.Translate("Error", isEnglish);
        string okBtn = LocalizationManager.Translate("Ok", isEnglish);

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Pin))
        {
            await page.DisplayAlert(errorTitle, LocalizationManager.Translate("ErrorNamePinRequired", isEnglish), okBtn);
            return;
        }

        if (Pin.Length < 4)
        {
            await page.DisplayAlert(errorTitle, LocalizationManager.Translate("ErrorPinLength", isEnglish), okBtn);
            return;
        }

        if (IsChef && SelectedConsumer == null)
        {
            await page.DisplayAlert(errorTitle, LocalizationManager.Translate("ErrorChefConsumerRequired", isEnglish), okBtn);
            return;
        }

        var postojeci = _db.GetUserByPin(Pin);
        if (postojeci != null)
        {
            await page.DisplayAlert(errorTitle, LocalizationManager.Translate("ErrorPinInUse", isEnglish), okBtn);
            return;
        }

        string uloga = IsChef ? "Chef" : "Consumer";
        string zaKoga = IsChef ? SelectedConsumer!.Name : string.Empty;

        var noviKorisnik = new User
        {
            Name = Name.Trim(),
            Pin = Pin.Trim(),
            Role = uloga,
            CooksFor = zaKoga
        };

        _db.SaveUser(noviKorisnik);

        string successTitle = LocalizationManager.Translate("Success", isEnglish);
        await page.DisplayAlert(successTitle, LocalizationManager.Translate("SuccessRegistered", isEnglish), okBtn);
        await page.Navigation.PopAsync();
    }
}