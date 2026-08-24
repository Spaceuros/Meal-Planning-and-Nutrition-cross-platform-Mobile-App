using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Models;
using SpejsMealPlanner.Data;
using System.Collections.ObjectModel;

namespace SpejsMealPlanner.ViewModels;

public partial class SelectMealViewModel : ObservableObject
{
    private readonly Database _db;

    public ObservableCollection<Meal> MenuMeals { get; set; } = new();

    public SelectMealViewModel(Database db)
    {
        _db = db;
    }

    public void UcitajMeni()
    {
        MenuMeals.Clear();

        var sviObroci = _db.GetMeals().Where(m => !m.IsDeleted).ToList();
        var sveNamirnice = _db.GetIngredients().ToDictionary(i => i.Id);

        var jedinstveniObroci = sviObroci.GroupBy(m => m.Name).Select(g => g.First()).ToList();

        foreach (var obrok in jedinstveniObroci)
        {
            int mealCalories = 0;
            var sastojciObroka = _db.GetIngredientsForMeal(obrok.Id);
            
            foreach (var mi in sastojciObroka)
            {
                if (sveNamirnice.TryGetValue(mi.IngredientId, out var namirnica))
                {
                    double factor = mi.Grams / 100.0;
                    mealCalories += (int)(namirnica.Calories * factor);
                }
            }
            
            obrok.Calories = mealCalories;
            MenuMeals.Add(obrok);
        }
    }

    [RelayCommand]
    private async Task OdaberiObrokAsync(Meal sablon)
    {
        if (sablon == null) return;

        var noviObrok = new Meal
        {
            Name = sablon.Name,
            Category = sablon.Category,
            OwnerName = sablon.OwnerName,
            Date = DateTime.Today 
        };

        var stariSastojci = _db.GetIngredientsForMeal(sablon.Id);
        var noviSastojci = new List<MealIngredient>();

        foreach (var s in stariSastojci)
        {
            noviSastojci.Add(new MealIngredient 
            { 
                IngredientId = s.IngredientId, 
                Grams = s.Grams 
            });
        }

        _db.SaveMeal(noviObrok, noviSastojci);

        await Shell.Current.Navigation.PopAsync();
    }
}