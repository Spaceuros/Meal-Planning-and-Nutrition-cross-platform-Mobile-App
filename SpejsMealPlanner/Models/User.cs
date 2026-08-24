using SQLite;

namespace SpejsMealPlanner.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Pin { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string CooksFor { get; set; } = string.Empty;
}