using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IdleGame.Models;

public class ResourceInfo
{
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }
	
	[JsonPropertyName("icon")]
	public string Icon { get; set; }
	
	[JsonPropertyName("description")]
	public string Description { get; set; }

	[JsonPropertyName("isGatherable")]
	public bool IsGatherable { get; set; }

	[JsonPropertyName("sellPrice")]
	public float SellPrice { get; set; }

	[JsonPropertyName("recipe")]
	public List<RecipeIngredient> Recipe { get; set; }
} 
