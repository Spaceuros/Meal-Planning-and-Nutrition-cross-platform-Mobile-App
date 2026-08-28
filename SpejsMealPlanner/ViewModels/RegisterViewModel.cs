using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Data;
using SpejsMealPlanner.Models;
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
    private string consumerButtonColor = "#3B82F6";

    [ObservableProperty]
    private string chefButtonColor = "#1E293B";

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
        ConsumerButtonColor = "#3B82F6";
        ChefButtonColor = "#1E293B";
    }

    [RelayCommand]
    private void SelectChef()
    {
        IsConsumer = false;
        IsChef = true;
        ConsumerButtonColor = "#1E293B";
        ChefButtonColor = "#3B82F6";
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

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Pin))
        {
            await page.DisplayAlert("Greška", "Ime i PIN su obavezna polja.", "OK");
            return;
        }

        if (Pin.Length < 4)
        {
            await page.DisplayAlert("Greška", "PIN mora imati barem 4 cifre.", "OK");
            return;
        }

        if (IsChef && SelectedConsumer == null)
        {
            await page.DisplayAlert("Greška", "Moraš izabrati konzumenta za koga kuvaš.", "OK");
            return;
        }

        var postojeci = _db.GetUserByPin(Pin);
        if (postojeci != null)
        {
            await page.DisplayAlert("Greška", "Ovaj PIN je već u upotrebi. Izaberi drugi.", "OK");
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

        await page.DisplayAlert("Uspeh", "Korisnik je uspešno registrovan!", "OK");
        await page.Navigation.PopAsync();
    }
}