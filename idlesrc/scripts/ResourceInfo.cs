using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace IdleGame;

public class RecipeIngredient
{
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("quantity")]
	public int Quantity { get; set; }
}

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

	[JsonPropertyName("recipe")]
	public List<RecipeIngredient> Recipe { get; set; }
} 
