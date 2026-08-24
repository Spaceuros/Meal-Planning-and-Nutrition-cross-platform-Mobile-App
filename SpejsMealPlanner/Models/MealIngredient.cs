using SQLite;

namespace SpejsMealPlanner.Models;

public class MealIngredient
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public int MealId { get; set; } 
    public int IngredientId { get; set; } 
    
    public int Grams { get; set; }
    
    [Ignore]
    public Ingredient Item { get; set; } = null!; // Dodato = null!
    
    [Ignore]
    public int CalculatedCalories { get; set; }
}