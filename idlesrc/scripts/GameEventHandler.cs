using Godot;
using IdleGame.Models;

namespace IdleGame;

public partial class GameEventHandler : Node
{
    public override void _Ready()
    {
        GameEvent.HireRequested += OnHireRequested;
    }

    public override void _ExitTree()
    {
        GameEvent.HireRequested -= OnHireRequested;
    }

    private void OnHireRequested(ResourceRequestModel request)
    {
        var resourceInfo = ResourceData.Instance.GetResourceById(request.ResourceId);
        if (resourceInfo == null)
        {
            GD.PrintErr($"Failed to load resource info for {request.ResourceId}");
            return;
        }

        var employeeCost = resourceInfo.SellPrice * 10;
        if (GameState.Instance.GetMoney() >= employeeCost)
        {
            GameState.Instance.AddMoney(-employeeCost);
            GameState.Instance.AddEmployee(request.ResourceId);
            GD.Print($"Hired an employee for {resourceInfo.Name} gathering!");
        }
    }
}