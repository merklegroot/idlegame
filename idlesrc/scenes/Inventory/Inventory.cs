using Godot;
using System.Collections.Generic;

namespace IdleGame;

public partial class Inventory : Control
{
	private VBoxContainer _resourceContainer;
	private Dictionary<string, Label> _resourceLabels = new();
	
	public override void _Ready()
	{
		_resourceContainer = GetNode<VBoxContainer>("VBoxContainer/Resources");

		// Create labels for each resource
		foreach (var resource in ResourceData.Instance.ListResources())
		{
			var label = new Label
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				ThemeTypeVariation = "",
			};
			
			// Set font size using theme override
			label.AddThemeConstantOverride("font_size", 32);
			
			_resourceContainer.AddChild(label);
			_resourceLabels[resource.Id] = label;
		}
		
		// Connect to inventory changes
		GameState.Instance.InventoryChanged += UpdateDisplay;
		
		UpdateDisplay();
	}
	
	private void UpdateDisplay()
	{
		foreach (var (resourceId, label) in _resourceLabels)
		{
			var resourceInfo = ResourceData.Instance.GetResourceById(resourceId);
			var quantity = GameState.Instance.GetResouceQuantity(resourceId);
			label.Text = $"{resourceInfo.Name}: {quantity}";
		}
	}

	public override void _ExitTree()
	{
		foreach (var label in _resourceLabels.Values)
		{
			if (label != null && IsInstanceValid(label))
			{
				label.QueueFree();
			}
		}
		_resourceLabels.Clear();
	}
} 