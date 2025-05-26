using System.Text.Json.Serialization;

namespace IdleGame.Models;

public class RecipeIngredient
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}