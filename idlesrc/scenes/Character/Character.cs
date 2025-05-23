using Godot;

namespace IdleGame;

public partial class Character : Control
{
    private VBoxContainer _statsContainer;
    
    public override void _Ready()
    {
        _statsContainer = GetNode<VBoxContainer>("VBoxContainer/Stats");
        InitializeStats();
    }

    private void InitializeStats()
    {
        // TODO: Add character stats initialization
        // This will be expanded as we add more character features
    }
} 