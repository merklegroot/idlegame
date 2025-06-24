using Godot;
using IdleGame.Models;
using IdleGame.Models.Messages;

namespace IdleGame;

public partial class InventoryItem : Button
{
	[Export]
	public string ResourceId { get; set; } = null;
	
	private TextureRect _icon;
	private Label _label;
	
	public override void _Ready()
	{
		// Get references to UI elements
		_icon = GetNode<TextureRect>("HBoxContainer/Icon");
		_label = GetNode<Label>("HBoxContainer/Label");
		
		// Update the display based on ResourceId
		if (!string.IsNullOrEmpty(ResourceId))
		{
			UpdateDisplay();
		}
	}
	
	private void UpdateDisplay()
	{
		var resourceInfo = ResourceData.Instance.GetResourceById(ResourceId);
		if (resourceInfo != null)
		{
			_icon.Texture = GD.Load<Texture2D>(resourceInfo.Icon);
			_label.Text = resourceInfo.Name;
		}
		else
		{
			_label.Text = ResourceId;
		}
	}
} 