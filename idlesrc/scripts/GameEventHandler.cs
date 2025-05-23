using Godot;
using IdleGame.Models;
using System.Text.Json;

namespace IdleGame;

public partial class GameEventHandler : Node
{
    public override void _Ready()
    {
        GD.Print("GameEvents ready");

        // Subscribe to the hire event
        GatherLine.HireRequested += OnHireRequested;
    }

    public override void _ExitTree()
    {
        // Unsubscribe from the hire event
        GatherLine.HireRequested -= OnHireRequested;
    }

    private void OnHireRequested(ResourceRequestModel request)
    {
        GD.Print($"Event handler - Received: {JsonSerializer.Serialize(request)}");
    }
}