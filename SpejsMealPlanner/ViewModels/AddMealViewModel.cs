using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Models;
using SpejsMealPlanner.Data; 
using SpejsMealPlanner.Utilities;
using System.Collections.ObjectModel;

namespace SpejsMealPlanner.ViewModels;

public partial class AddMealViewModel : ObservableObject
{
    private readonly Database _db; 
    private Meal _currentMeal = null!;

    [ObservableProperty]
    private string mealName = string.Empty;

    [ObservableProperty]
    private string mealCategory = string.Empty;

    [ObservableProperty]
    private string gramsText = string.Empty;

    [ObservableProperty]
    private Ingredient? selectedIngredient;

    [ObservableProperty]
    private string saveButtonText = string.Empty;

    public ObservableCollection<MealIngredient> CurrentIngredients { get; set; } = new();
    public List<Ingredient> AvailableIngredients { get; set; } = new();

    public AddMealViewModel(Database db)
    {
        _db = db;
    }

    public void PripremiObrok(Meal? obrokZaIzmenu = null)
    {
        AvailableIngredients = _db.GetIngredients();
        bool isEnglish = Preferences.Default.Get("IsEnglish", false);

        if (obrokZaIzmenu == null)
        {
            _currentMeal = new Meal { Date = DateTime.Today };
            SaveButtonText = LocalizationManager.Translate("Save", isEnglish);
        }
        else
        {
            _currentMeal = obrokZaIzmenu;
            MealName = _currentMeal.Name;
            MealCategory = _currentMeal.Category;
            SaveButtonText = LocalizationManager.Translate("Update", isEnglish);

            UcitajPostojeceSastojke();
        }
    }

    private void UcitajPostojeceSastojke()
    {
        var sastojciIzBaze = _db.GetIngredientsForMeal(_currentMeal.Id);
        var sveNamirnice = _db.GetIngredients().ToDictionary(i => i.Id);
        
        foreach (var mi in sastojciIzBaze)
        {
            if (sveNamirnice.TryGetValue(mi.IngredientId, out var namirnica))
            {
                mi.Item = namirnica;
                CurrentIngredients.Add(mi);
            }
        }
    }

    [RelayCommand]
    private void AddIngredient()
    {
        if (SelectedIngredient != null && int.TryParse(GramsText, out int grami))
        {
            var noviSastojak = new MealIngredient
            {
                IngredientId = SelectedIngredient.Id,
                Grams = grami,
                Item = SelectedIngredient
            };
            
            CurrentIngredients.Add(noviSastojak);
            
            SelectedIngredient = null;
            GramsText = string.Empty;
        }
    }

    [RelayCommand]
    private void RemoveIngredient(MealIngredient sastojakZaBrisanje)
    {
        if (sastojakZaBrisanje != null)
        {
            CurrentIngredients.Remove(sastojakZaBrisanje);
        }
    }

    [RelayCommand]
    private async Task SaveMealAsync()
    {
        bool isEnglish = Preferences.Default.Get("IsEnglish", false);

        if (string.IsNullOrWhiteSpace(MealName) || CurrentIngredients.Count == 0)
        {
            string errorTitle = LocalizationManager.Translate("Error", isEnglish);
            string errorMessage = isEnglish ? "Enter meal name and add at least one ingredient." : "Unesi naziv obroka i dodaj barem jedan sastojak.";
            await Shell.Current.DisplayAlert(errorTitle, errorMessage, LocalizationManager.Translate("Ok", isEnglish));
            return;
        }

        string uloga = Preferences.Default.Get("UserRole", "Solo");
        string kuvaZa = Preferences.Default.Get("CooksFor", string.Empty);
        string trenutnoIme = Preferences.Default.Get("UserName", "Solo");

        _currentMeal.Name = MealName;
        _currentMeal.Category = MealCategory ?? string.Empty;
        _currentMeal.OwnerName = uloga == "Chef" ? kuvaZa : trenutnoIme;

        _db.SaveMeal(_currentMeal, CurrentIngredients.ToList());

        string successTitle = LocalizationManager.Translate("Success", isEnglish);
        string successMessage = isEnglish ? "Meal successfully saved!" : "Obrok je uspešno sačuvan!";
        await Shell.Current.DisplayAlert(successTitle, successMessage, LocalizationManager.Translate("Ok", isEnglish));
        await Shell.Current.Navigation.PopAsync();
    }
}