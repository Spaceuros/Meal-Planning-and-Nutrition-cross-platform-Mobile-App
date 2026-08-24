using SQLite;
using SpejsMealPlanner.Models;

namespace SpejsMealPlanner.Data;

public class Database
{
    private readonly SQLiteConnection _connection;

    public Database()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "SpejsMealPlanner.db3");
        _connection = new SQLiteConnection(dbPath);
        
        _connection.CreateTable<Ingredient>();
        _connection.CreateTable<Meal>();
        _connection.CreateTable<MealIngredient>();
        _connection.CreateTable<User>();
    }

    public List<Ingredient> GetIngredients()
    {
        return _connection.Table<Ingredient>().ToList();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        _connection.Insert(ingredient);
    }

    public void DeleteIngredient(Ingredient ingredient)
    {
        _connection.Delete(ingredient);
    }

    public void SaveIngredient(Ingredient ingredient)
    {
        if (ingredient.Id != 0)
        {
            _connection.Update(ingredient);
        }
        else
        {
            _connection.Insert(ingredient);
        }
    }

    public void DeleteIngredient(int ingredientId)
    {
        _connection.Table<MealIngredient>().Where(mi => mi.IngredientId == ingredientId).Delete();
        _connection.Delete<Ingredient>(ingredientId);
    }

    public List<Meal> GetMeals()
    {
        string uloga = Preferences.Default.Get("UserRole", "Solo");
        string trenutnoIme = Preferences.Default.Get("UserName", "Solo");
        string kuvaZa = Preferences.Default.Get("CooksFor", string.Empty);

        string ciljniVlasnik = uloga == "Chef" ? kuvaZa : trenutnoIme;

        return _connection.Table<Meal>()
                          .Where(m => m.OwnerName == ciljniVlasnik || m.OwnerName == "")
                          .ToList();
    }

    public void AddMeal(Meal meal)
    {
        _connection.Insert(meal);
    }

    public void DeleteMeal(int mealId)
    {
        _connection.Table<MealIngredient>().Where(mi => mi.MealId == mealId).Delete();
        _connection.Delete<Meal>(mealId);
    }

    public void SaveMeal(Meal meal, List<MealIngredient> ingredients)
    {
        if (meal.Id != 0)
        {
            _connection.Update(meal);
            _connection.Table<MealIngredient>().Where(mi => mi.MealId == meal.Id).Delete();
        }
        else
        {
            _connection.Insert(meal);
        }

        foreach (var ing in ingredients)
        {
            ing.MealId = meal.Id;
            _connection.Insert(ing);
        }
    }

    public void SoftDeleteMeal(int id)
    {
        var meal = _connection.Table<Meal>().FirstOrDefault(m => m.Id == id);
        if (meal != null)
        {
            meal.IsDeleted = true;
            meal.DeletedAt = DateTime.Now;
            _connection.Update(meal);

            var obrisani = _connection.Table<Meal>().Where(m => m.IsDeleted).OrderBy(m => m.DeletedAt).ToList();
            
            if (obrisani.Count > 5)
            {
                var zaTrajnoBrisanje = obrisani.Take(obrisani.Count - 5);
                foreach (var stariObrok in zaTrajnoBrisanje)
                {
                    _connection.Execute("DELETE FROM MealIngredient WHERE MealId = ?", stariObrok.Id);
                    _connection.Delete(stariObrok);
                }
            }
        }
    }

    public List<Meal> GetDeletedMeals()
    {
        string uloga = Preferences.Default.Get("UserRole", "Solo");
        string trenutnoIme = Preferences.Default.Get("UserName", "Solo");
        string kuvaZa = Preferences.Default.Get("CooksFor", string.Empty);

        string ciljniVlasnik = uloga == "Chef" ? kuvaZa : trenutnoIme;

        return _connection.Table<Meal>()
                          .Where(m => m.IsDeleted && (m.OwnerName == ciljniVlasnik || m.OwnerName == ""))
                          .OrderByDescending(m => m.DeletedAt)
                          .ToList();
    }

    public void RestoreMeal(int id)
    {
        var meal = _connection.Table<Meal>().FirstOrDefault(m => m.Id == id);
        if (meal != null)
        {
            meal.IsDeleted = false;
            _connection.Update(meal);
        }
    }

    public void AddMealIngredient(MealIngredient mealIngredient)
    {
        _connection.Insert(mealIngredient);
    }

    public List<MealIngredient> GetIngredientsForMeal(int mealId)
    {
        return _connection.Table<MealIngredient>().Where(mi => mi.MealId == mealId).ToList();
    }

    public List<User> GetUsers()
    {
        return _connection.Table<User>().ToList();
    }

    public User? GetUserByPin(string pin)
    {
        return _connection.Table<User>().FirstOrDefault(u => u.Pin == pin);
    }

    public int SaveUser(User user)
    {
        if (user.Id != 0)
        {
            return _connection.Update(user);
        }
        else
        {
            return _connection.Insert(user);
        }
    }

    public int DeleteUser(int id)
    {
        return _connection.Delete<User>(id);
    }

    public List<User> GetConsumers()
    {
        return _connection.Table<User>()
                          .Where(u => u.Role == "Consumer")
                          .ToList();
    }
    public void UpdateMealOnly(Meal meal)
    {
        _connection.Update(meal);
    }
}