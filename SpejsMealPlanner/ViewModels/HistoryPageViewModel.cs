using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Models;
using SpejsMealPlanner.Data;
using System.Collections.ObjectModel;

namespace SpejsMealPlanner.ViewModels;

public partial class HistoryPageViewModel : ObservableObject
{
    private readonly Database _db; 

    public ObservableCollection<Meal> DeletedMeals { get; set; } = new();

    public HistoryPageViewModel(Database db)
    {
        _db = db;
    }

    public void UcitajIstoriju()
    {
        DeletedMeals.Clear();
        var obrisaniObroci = _db.GetDeletedMeals();
        var sveNamirnice = _db.GetIngredients().ToDictionary(i => i.Id);

        foreach (var obrok in obrisaniObroci)
        {
            int mealCalories = 0;
            var sastojciObroka = _db.GetIngredientsForMeal(obrok.Id);

            foreach (var mi in sastojciObroka)
            {
                if (sveNamirnice.TryGetValue(mi.IngredientId, out var namirnica))
                {
                    mi.Item = namirnica;
                    double factor = mi.Grams / 100.0;
                    mealCalories += (int)(namirnica.Calories * factor);
                }
            }
            obrok.Calories = mealCalories;
            DeletedMeals.Add(obrok);
        }
    }

    [RelayCommand]
    private void RestoreMeal(Meal obrokZaVracanje)
    {
        if (obrokZaVracanje == null) return;

        _db.RestoreMeal(obrokZaVracanje.Id);
        
        UcitajIstoriju(); 
    }
}