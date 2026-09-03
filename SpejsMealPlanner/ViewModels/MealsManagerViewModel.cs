using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Models;
using SpejsMealPlanner.Data; 
using SpejsMealPlanner.Utilities;
using System.Collections.ObjectModel;

namespace SpejsMealPlanner.ViewModels;

public partial class MealsManagerViewModel : ObservableObject
{
    private readonly Database _db; 

    public ObservableCollection<Meal> ManagerMeals { get; set; } = new();

    public MealsManagerViewModel(Database db)
    {
        _db = db;
    }

    public void UcitajObroke()
    {
        ManagerMeals.Clear();
        
        var sviObroci = _db.GetMeals().Where(m => !m.IsDeleted).ToList();
        var sveNamirnice = _db.GetIngredients().ToDictionary(i => i.Id);

        foreach (var obrok in sviObroci)
        {
            int mealCalories = 0;
            var sastojciObroka = _db.GetIngredientsForMeal(obrok.Id);
            obrok.Ingredients.Clear(); 

            foreach (var mi in sastojciObroka)
            {
                if (sveNamirnice.TryGetValue(mi.IngredientId, out var namirnica))
                {
                    mi.Item = namirnica;
                    
                    double factor = mi.Grams / 100.0;
                    mi.CalculatedCalories = (int)(namirnica.Calories * factor);
                    
                    mealCalories += mi.CalculatedCalories;
                    
                    obrok.Ingredients.Add(mi);
                }
            }
            
            obrok.Calories = mealCalories;
            ManagerMeals.Add(obrok);
        }
    }

    [RelayCommand]
    private async Task DeleteMealAsync(Meal obrokZaBrisanje)
    {
        if (obrokZaBrisanje == null) return;
        bool isEnglish = Preferences.Default.Get("IsEnglish", false);
        
        string title = isEnglish ? "Confirm" : "Potvrda";
        string message = isEnglish ? $"Do you want to delete '{obrokZaBrisanje.Name}'?" : $"Da li želiš da obrišeš '{obrokZaBrisanje.Name}'?";
        string yesBtn = isEnglish ? "Yes" : "Da";
        string noBtn = isEnglish ? "Cancel" : "Odustani";

        bool potvrda = await Shell.Current.DisplayAlert(title, message, yesBtn, noBtn);
        if (potvrda)
        {
            _db.SoftDeleteMeal(obrokZaBrisanje.Id); 
            UcitajObroke();
        }
    }

    [RelayCommand]
    private async Task EditMealAsync(Meal obrokZaIzmenu)
    {
        if (obrokZaIzmenu == null) return;
        await Shell.Current.Navigation.PushAsync(new AddMealPage(obrokZaIzmenu));
    }

    [RelayCommand]
    private async Task AddNewMealAsync()
    {
        await Shell.Current.Navigation.PushAsync(new AddMealPage());
    }

    [RelayCommand]
    private async Task OpenHistoryAsync()
    {
        var vm = Application.Current?.Handler?.MauiContext?.Services.GetService<HistoryPageViewModel>();
        if (vm != null)
        {
            await Shell.Current.Navigation.PushAsync(new HistoryPage(vm));
        }
    }

    [RelayCommand]
    private void Logout()
    {
        Preferences.Default.Remove("UserRole");
        Preferences.Default.Remove("UserName");
        Preferences.Default.Remove("CooksFor");
        
        var loginPage = Application.Current!.Handler!.MauiContext!.Services.GetService<LoginPage>();
        Application.Current!.Windows[0].Page = new NavigationPage(loginPage);
    }
}