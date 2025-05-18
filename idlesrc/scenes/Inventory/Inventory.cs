using Godot;
using System.Collections.Generic;

namespace IdleGame;

public partial class Inventory : Control
{
	private VBoxContainer _resourceContainer;
	private List<InventoryLine> _resourceLines = new();
	
	public override void _Ready()
	{
		_resourceContainer = GetNode<VBoxContainer>("VBoxContainer/Resources");
		var inventoryLinePath = "res://scenes/Inventory/InventoryLine.tscn";
		var inventoryLineScene = GD.Load<PackedScene>(inventoryLinePath);

		// Clean up any existing lines
		foreach (var line in _resourceLines)
		{
			if (line != null && IsInstanceValid(line))
			{
				line.QueueFree();
			}
		}
		_resourceLines.Clear();

		// Create lines for each resource
		foreach (var resource in ResourceData.Instance.ListResources())
		{
			var line = inventoryLineScene.Instantiate<InventoryLine>();
			line.ResourceId = resource.Id;
			_resourceContainer.AddChild(line);
			_resourceLines.Add(line);
		}
	}

	public override void _ExitTree()
	{
		foreach (var line in _resourceLines)
		{
			if (line != null && IsInstanceValid(line))
			{
				line.QueueFree();
			}
		}
		_resourceLines.Clear();
	}
} 