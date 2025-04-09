using System;

namespace App.Model;

public class ApiKey
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty; // Unique API key
    public string User { get; set; } = string.Empty; // Optional: Associate with a user or app
    public int RemainingLimit { get; set; } // API request limit
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
