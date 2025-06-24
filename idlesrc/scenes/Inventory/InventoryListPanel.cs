using Godot;
using System.Collections.Generic;
using IdleGame.Models;
using IdleGame.Models.Messages;

namespace IdleGame;

public partial class InventoryListPanel : Control
{
    private VBoxContainer _resourceContainer;
    // private List<InventoryListItem> _resourceLines = new();
    private List<InventoryItem> _resourceLines = new();
    
    public override void _Ready()
    {
        _resourceContainer = GetNode<VBoxContainer>("Panel/VBoxContainer");
        // var inventoryLinePath = "res://scenes/Inventory/InventoryListItem.tscn";
        var inventoryLinePath = "res://scenes/Inventory/InventoryItem.tscn";
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
            // var line = inventoryLineScene.Instantiate<InventoryListItem>();
            var line = inventoryLineScene.Instantiate<InventoryItem>();
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
        GD.Print($"InventoryListPanel: Inventory changed: {request.ResourceId} {request.Quantity}");
    }
} 