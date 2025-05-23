using Godot;
using IdleGame.Models;
using System.Text.Json;

namespace IdleGame;

public partial class GameEvents : Node
{
    public override void _Ready()
    {
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
        GD.Print($"Hire requested for resource: {JsonSerializer.Serialize(request)}");
    }
}