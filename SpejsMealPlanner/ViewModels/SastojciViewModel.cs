using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Models;
using SpejsMealPlanner.Data;
using SpejsMealPlanner.Utilities;
using System.Collections.ObjectModel;

namespace SpejsMealPlanner.ViewModels;

public partial class SastojciViewModel : ObservableObject
{
    private readonly Database _db;

    public ObservableCollection<Ingredient> Ingredients { get; set; } = new();

    [ObservableProperty]
    private string nameEntryText = string.Empty;

    [ObservableProperty]
    private string caloriesEntryText = string.Empty;

    [ObservableProperty]
    private string proteinEntryText = string.Empty;

    [ObservableProperty]
    private string carbsEntryText = string.Empty;

    [ObservableProperty]
    private string fatsEntryText = string.Empty;

    public SastojciViewModel(Database db)
    {
        _db = db;
    }

    public void UcitajSastojke()
    {
        Ingredients.Clear();
        var izBaze = _db.GetIngredients();
        foreach (var item in izBaze)
        {
            Ingredients.Add(item);
        }
    }

    [RelayCommand]
    private async Task AddIngredientAsync()
    {
        bool isEnglish = Preferences.Default.Get("IsEnglish", false);
        if (string.IsNullOrWhiteSpace(NameEntryText) || string.IsNullOrWhiteSpace(CaloriesEntryText))
        {
            string title = LocalizationManager.Translate("Error", isEnglish);
            string msg = isEnglish ? "Enter at least name and calories." : "Unesi barem naziv i kalorije.";
            await Shell.Current.DisplayAlert(title, msg, LocalizationManager.Translate("Ok", isEnglish));
            return;
        }

        double ParseDouble(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            string cleanText = text.Replace(',', '.');
            return double.TryParse(cleanText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double result) ? result : 0;
        }
        
        int ParseInt(string text) => int.TryParse(text, out int result) ? result : 0;

        string unetoIme = NameEntryText.Trim();
        
        var postojeciSastojak = _db.GetIngredients()
                                      .FirstOrDefault(i => i.Name.Equals(unetoIme, StringComparison.OrdinalIgnoreCase));

        if (postojeciSastojak != null)
        {
            postojeciSastojak.Calories = ParseInt(CaloriesEntryText);
            postojeciSastojak.Protein = ParseDouble(ProteinEntryText);
            postojeciSastojak.Carbs = ParseDouble(CarbsEntryText);
            postojeciSastojak.Fats = ParseDouble(FatsEntryText);
            _db.SaveIngredient(postojeciSastojak);
        }
        else
        {
            var noviSastojak = new Ingredient
            {
                Name = unetoIme,
                Calories = ParseInt(CaloriesEntryText),
                Protein = ParseDouble(ProteinEntryText),
                Carbs = ParseDouble(CarbsEntryText),
                Fats = ParseDouble(FatsEntryText)
            };
            _db.SaveIngredient(noviSastojak);
        }
        
        NameEntryText = string.Empty;
        CaloriesEntryText = string.Empty;
        ProteinEntryText = string.Empty;
        CarbsEntryText = string.Empty;
        FatsEntryText = string.Empty;

        UcitajSastojke();
    }

    [RelayCommand]
    private void EditIngredient(Ingredient sastojak)
    {
        if (sastojak != null)
        {
            NameEntryText = sastojak.Name;
            CaloriesEntryText = sastojak.Calories.ToString();
            ProteinEntryText = sastojak.Protein.ToString();
            CarbsEntryText = sastojak.Carbs.ToString();
            FatsEntryText = sastojak.Fats.ToString();
        }
    }

    [RelayCommand]
    private async Task DeleteIngredientAsync(Ingredient sastojak)
    {
        if (sastojak == null) return;
        bool isEnglish = Preferences.Default.Get("IsEnglish", false);
        
        string title = isEnglish ? "Delete" : "Brisanje";
        string msg = isEnglish ? $"Do you want to delete '{sastojak.Name}'?" : $"Da li želiš da obrišeš '{sastojak.Name}'?";
        string yesBtn = isEnglish ? "Yes" : "Da";
        string noBtn = isEnglish ? "No" : "Ne";

        bool potvrda = await Shell.Current.DisplayAlert(title, msg, yesBtn, noBtn);
        if (potvrda)
        {
            _db.DeleteIngredient(sastojak.Id);
            UcitajSastojke();
        }
    }
}