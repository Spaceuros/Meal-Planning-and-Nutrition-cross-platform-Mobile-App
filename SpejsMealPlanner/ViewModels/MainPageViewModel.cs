using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpejsMealPlanner.Models;
using SpejsMealPlanner.Data; 
using System.Collections.ObjectModel;

namespace SpejsMealPlanner.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly Database _db;

    public ObservableCollection<Meal> Meals { get; set; } = new();

    [ObservableProperty]
    private string dateText = string.Empty;

    [ObservableProperty]
    private string goalText = string.Empty;

    [ObservableProperty]
    private string totalCaloriesText = string.Empty;

    [ObservableProperty]
    private double caloriesProgress;

    [ObservableProperty]
    private string proteinText = string.Empty;

    [ObservableProperty]
    private string carbsText = string.Empty;

    [ObservableProperty]
    private string fatsText = string.Empty;

    [ObservableProperty]
    private int waterGlasses;

    [ObservableProperty]
    private string waterText = string.Empty;

    [ObservableProperty]
    private string waterGoalText = string.Empty;

    [ObservableProperty]
    private double waterProgress;

    public MainPageViewModel(Database db)
    {
        _db = db;
    }

    private void OsveziVodu()
    {
        string userName = Preferences.Default.Get("UserName", "Solo");
        int ciljVodeMl = Preferences.Default.Get($"DailyWaterGoal_{userName}", 2000); 
        int trenutnoMl = WaterGlasses * 250;
        
        WaterText = $"{trenutnoMl} ml";
        WaterGoalText = $"od {ciljVodeMl} ml";
        WaterProgress = Math.Min((double)trenutnoMl / ciljVodeMl, 1.0);
    }

    public void UcitajPodatke()
    {
        string userName = Preferences.Default.Get("UserName", "Solo");

        string danNaSrpskom = DateTime.Now.DayOfWeek switch
        {
            DayOfWeek.Monday => "Ponedeljak", DayOfWeek.Tuesday => "Utorak",
            DayOfWeek.Wednesday => "Sreda", DayOfWeek.Thursday => "Četvrtak",
            DayOfWeek.Friday => "Petak", DayOfWeek.Saturday => "Subota",
            DayOfWeek.Sunday => "Nedelja", _ => ""
        };
        DateText = $"{danNaSrpskom}, {DateTime.Now:dd. MMM}";

        string danasnjiDatum = DateTime.Today.ToString("yyyy-MM-dd");
        string sacuvaniDatum = Preferences.Default.Get($"LastWaterDate_{userName}", string.Empty);

        if (sacuvaniDatum != danasnjiDatum)
        {
            Preferences.Default.Set($"WaterGlasses_{userName}", 0);
            Preferences.Default.Set($"LastWaterDate_{userName}", danasnjiDatum);
        }
        
        WaterGlasses = Preferences.Default.Get($"WaterGlasses_{userName}", 0);
        OsveziVodu();

        Meals.Clear();
        int totalCalories = 0;
        double totalProteins = 0;
        double totalCarbs = 0;
        double totalFats = 0;

        var sacuvaniObroci = _db.GetMeals().Where(m => !m.IsDeleted).ToList();
        var sveNamirnice = _db.GetIngredients().ToDictionary(i => i.Id);

        foreach (var obrok in sacuvaniObroci)
        {
            if (obrok.Date.Date < DateTime.Today)
            {
                if (obrok.IsEaten)
                {
                    _db.SoftDeleteMeal(obrok.Id);
                    continue; 
                }
                else
                {
                    obrok.Date = DateTime.Today;
                    _db.UpdateMealOnly(obrok);
                }
            }

            int mealCalories = 0;
            double mealProteins = 0;
            double mealCarbs = 0;
            double mealFats = 0;

            var sastojciObroka = _db.GetIngredientsForMeal(obrok.Id);
            obrok.Ingredients.Clear();

            foreach (var mi in sastojciObroka)
            {
                if (sveNamirnice.TryGetValue(mi.IngredientId, out var namirnica))
                {
                    mi.Item = namirnica;
                    double factor = mi.Grams / 100.0;
                    
                    mealCalories += (int)(namirnica.Calories * factor);
                    mealProteins += namirnica.Protein * factor;
                    mealCarbs += namirnica.Carbs * factor;
                    mealFats += namirnica.Fats * factor;
                    
                    obrok.Ingredients.Add(mi);
                }
            }

            obrok.Calories = mealCalories;
            obrok.MealProtein = mealProteins;
            obrok.MealCarbs = mealCarbs;
            obrok.MealFats = mealFats;

            if (obrok.IsEaten)
            {
                totalCalories += mealCalories;
                totalProteins += mealProteins;
                totalCarbs += mealCarbs;
                totalFats += mealFats;
            }

            Meals.Add(obrok);
        }

        int dnevniCilj = Preferences.Default.Get($"DailyCalorieGoal_{userName}", 2000);

        TotalCaloriesText = totalCalories.ToString();
        GoalText = $"od {dnevniCilj} kcal uneto";
        CaloriesProgress = Math.Min((double)totalCalories / dnevniCilj, 1.0);
        
        ProteinText = totalProteins.ToString("F1");
        CarbsText = totalCarbs.ToString("F1");
        FatsText = totalFats.ToString("F1");
    }

    [RelayCommand]
    private void ToggleEaten(Meal obrok)
    {
        if (obrok == null) return;
        
        obrok.IsEaten = !obrok.IsEaten;
        _db.UpdateMealOnly(obrok);
        
        UcitajPodatke(); 
    }

    [RelayCommand]
    private void ToggleExpand(Meal obrok)
    {
        if (obrok == null) return;
        
        obrok.IsExpanded = !obrok.IsExpanded;
    }

    [RelayCommand]
    private async Task ChangeGoalAsync()
    {
        string userName = Preferences.Default.Get("UserName", "Solo");
        int trenutniCilj = Preferences.Default.Get($"DailyCalorieGoal_{userName}", 2000);
        
        string? rezultat = await Shell.Current.DisplayPromptAsync(
            "Dnevni cilj", "Unesi svoj novi dnevni cilj kalorija:", 
            initialValue: trenutniCilj.ToString(), keyboard: Keyboard.Numeric);

        if (int.TryParse(rezultat, out int noviCilj) && noviCilj > 0)
        {
            Preferences.Default.Set($"DailyCalorieGoal_{userName}", noviCilj);
            UcitajPodatke(); 
        }
    }

    [RelayCommand]
    private async Task DeleteMealAsync(Meal obrokZaBrisanje)
    {
        if (obrokZaBrisanje == null) return;

        bool potvrda = await Shell.Current.DisplayAlert(
            "Brisanje", $"Da li želiš da obrišeš '{obrokZaBrisanje.Name}'?", "Da", "Odustani");
        
        if (potvrda)
        {
            _db.SoftDeleteMeal(obrokZaBrisanje.Id);
            UcitajPodatke();
        }
    }

    [RelayCommand]
    private async Task AddMealAsync()
    {
        string uloga = Preferences.Default.Get("UserRole", "Solo");

        if (uloga == "Consumer")
        {
            var vm = Application.Current?.Handler?.MauiContext?.Services.GetService<SelectMealViewModel>();
            if (vm != null)
            {
                await Shell.Current.Navigation.PushAsync(new SelectMealPage(vm));
            }
        }
        else
        {
            await Shell.Current.Navigation.PushAsync(new AddMealPage());
        }
    }

    [RelayCommand]
    private void AddWater()
    {
        string userName = Preferences.Default.Get("UserName", "Solo");
        WaterGlasses++;
        Preferences.Default.Set($"WaterGlasses_{userName}", WaterGlasses);
        OsveziVodu();
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