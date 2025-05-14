using System.Text.Json.Serialization;

namespace IdleGame;

public class ResourceInfo
{
	[JsonPropertyName("name")]
	public string Name { get; set; }
	
	[JsonPropertyName("icon")]
	public string Icon { get; set; }
	
	[JsonPropertyName("description")]
	public string Description { get; set; }
} 
