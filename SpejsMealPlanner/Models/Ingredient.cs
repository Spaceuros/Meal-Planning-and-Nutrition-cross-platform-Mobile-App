using SQLite;

namespace SpejsMealPlanner.Models;

public class Ingredient
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    // Sve vrednosti se odnose na 100 grama namirnice
    public int Calories { get; set; } 
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fats { get; set; }
}