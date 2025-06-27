using Godot;
using IdleGame.Models.Messages;

namespace IdleGame;

public partial class InventoryDetailsPanel : Control
{
    public override void _Ready()
    {
        GameEvent.InventoryItemSelected += OnInventoryItemSelected;
    }

    private void OnInventoryItemSelected(InventoryItemSelectedMessage message)
    {

        GD.Print($"InventoryDetailsPanel - OnInventoryItemSelected: {message.ResourceId}");
    }
} 