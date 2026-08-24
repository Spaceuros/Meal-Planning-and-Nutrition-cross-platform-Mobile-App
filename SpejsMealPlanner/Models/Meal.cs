using SQLite;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SpejsMealPlanner.Models;

// Nasleđujemo ObservableObject kako bi UI reagovao na promene u realnom vremenu
public partial class Meal : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;

    // Status za bazu - da li je obrok pojeden
    private bool _isEaten;
    public bool IsEaten 
    { 
        get => _isEaten; 
        set => SetProperty(ref _isEaten, value); 
    }
    
    public bool IsDeleted { get; set; } = false;
    public DateTime DeletedAt { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;

    private bool _isExpanded;

    [Ignore]
    public bool IsExpanded 
    { 
        get => _isExpanded; 
        set => SetProperty(ref _isExpanded, value); 
    }
    [Ignore]
    public int Calories { get; set; }

    [Ignore]
    public double MealProtein { get; set; }
    [Ignore]
    public double MealCarbs { get; set; }
    [Ignore]
    public double MealFats { get; set; }

    [Ignore]
    public ObservableCollection<MealIngredient> Ingredients { get; set; } = new();
}