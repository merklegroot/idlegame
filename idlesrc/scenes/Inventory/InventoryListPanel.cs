using Godot;
using System.Collections.Generic;
using IdleGame.Models;
using IdleGame.Models.Messages;

namespace IdleGame;

public partial class InventoryListPanel : Control
{
    private VBoxContainer _resourceContainer;
    private List<InventoryListItem> _resourceLines = new();
    private float _updateTimer = 0f;
    private const float UPDATE_INTERVAL = 1.0f; // 1 second

    private const string inventoryLinePath = "res://scenes/Inventory/InventoryListItem.tscn";

    private PackedScene inventoryLineScene;

    
    public override void _Ready()
    {
        _resourceContainer = GetNode<VBoxContainer>("Panel/VBoxContainer");        
        inventoryLineScene = GD.Load<PackedScene>(inventoryLinePath);

        UpdateDisplay();

        GameEvent.InventoryChanged += OnInventoryChanged;
    }

    public override void _ExitTree()
    {
        CleanupLines();

        GameEvent.InventoryChanged -= OnInventoryChanged;
    }

    public override void _Process(double delta)
    {
        _updateTimer += (float)delta;
        if (_updateTimer >= UPDATE_INTERVAL)
        {
            _updateTimer = 0f;
            UpdateDisplay();
        }
    }

    private void OnInventoryChanged(InventoryChangedMessage request)
    {
        GD.Print($"InventoryListPanel: Inventory changed: {request.ResourceId} {request.Quantity}");
        UpdateDisplay();        
    }

    private void CleanupLines()
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

    private void AddLines()
    {
        var resources = ResourceData.Instance.ListResources();

        foreach (var resource in resources)
        {
            var quantity = GameState.Instance.GetResourceQuantity(resource.Id);
            if (quantity <= 0)
                continue;
            
            var line = inventoryLineScene.Instantiate<InventoryListItem>();
            line.ResourceId = resource.Id;
            _resourceContainer.AddChild(line);
            _resourceLines.Add(line);
        }
    }

    private void UpdateDisplay()
    {
        CleanupLines();
        AddLines();
    }
} 