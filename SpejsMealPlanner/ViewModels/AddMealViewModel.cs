using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Models;
using SpejsMealPlanner.Data; 
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
    private string saveButtonText = "Sačuvaj";

    public ObservableCollection<MealIngredient> CurrentIngredients { get; set; } = new();
    public List<Ingredient> AvailableIngredients { get; set; } = new();

    public AddMealViewModel(Database db)
    {
        _db = db;
    }

    public void PripremiObrok(Meal? obrokZaIzmenu = null)
    {
        AvailableIngredients = _db.GetIngredients();

        if (obrokZaIzmenu == null)
        {
            _currentMeal = new Meal { Date = DateTime.Today };
            SaveButtonText = "Sačuvaj";
        }
        else
        {
            _currentMeal = obrokZaIzmenu;
            MealName = _currentMeal.Name;
            MealCategory = _currentMeal.Category;
            SaveButtonText = "Ažuriraj Obrok";

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
        if (string.IsNullOrWhiteSpace(MealName) || CurrentIngredients.Count == 0)
        {
            await Shell.Current.DisplayAlert("Greška", "Unesi naziv obroka i dodaj barem jedan sastojak.", "OK");
            return;
        }

        string uloga = Preferences.Default.Get("UserRole", "Solo");
        string kuvaZa = Preferences.Default.Get("CooksFor", string.Empty);
        string trenutnoIme = Preferences.Default.Get("UserName", "Solo");

        _currentMeal.Name = MealName;
        _currentMeal.Category = MealCategory ?? string.Empty;
        _currentMeal.OwnerName = uloga == "Chef" ? kuvaZa : trenutnoIme;

        _db.SaveMeal(_currentMeal, CurrentIngredients.ToList());

        await Shell.Current.DisplayAlert("Uspeh", "Obrok je uspešno sačuvan!", "OK");
        await Shell.Current.Navigation.PopAsync();
    }
}