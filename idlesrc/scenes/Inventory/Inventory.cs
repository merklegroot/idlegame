using Godot;
using System.Collections.Generic;
using IdleGame.Models;
using IdleGame.Models.Messages;

namespace IdleGame;

public partial class Inventory : Control
{
	private VBoxContainer _resourceContainer;
	private List<InventoryListItem> _resourceLines = new();
	
	public override void _Ready()
	{
		_resourceContainer = GetNode<VBoxContainer>("VBoxContainer/HSplitContainer/InventoryListSectionContainer/InventoryListItemContainer");
		var inventoryLinePath = "res://scenes/Inventory/InventoryListItem.tscn";
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
			var line = inventoryLineScene.Instantiate<InventoryListItem>();
			line.ResourceId = resource.Id;
			_resourceContainer.AddChild(line);
			_resourceLines.Add(line);
		}

		GameEvent.InventoryChanged += OnInventoryChanged;
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

	private void OnInventoryChanged(InventoryChangedMessage request)
	{
		GD.Print($"Inventory changed: {request.ResourceId} {request.Quantity}");
	}
} 