using Godot;
using IdleGame.Models;
using IdleGame.Models.Messages;

namespace IdleGame;

public partial class InventoryItem : Button
{
	[Export]
	public string ResourceId { get; set; } = null;
	
	private Label _label;

	private ResourceInfo _resourceInfo;
	
	public override void _Ready()
	{
		if (string.IsNullOrEmpty(ResourceId))
			return;

		// Get reference to the label
		_label = GetNode<Label>("Label");

		// Get resource info
		_resourceInfo = ResourceData.Instance.GetResourceById(ResourceId);
		if (_resourceInfo == null)
		{
			GD.PrintErr($"Failed to load resource info for {ResourceId}");
			return;
		}

		_label.Text = _resourceInfo.Name;
	}
} 