using Godot;
using System.Collections.Generic;

namespace IdleGame;

public partial class CraftPanel : Control
{
    private VBoxContainer _recipeContainer;
    private List<Control> _recipeLines = new();

    public override void _Ready()
    {
        _recipeContainer = GetNode<VBoxContainer>("VBoxContainer/Recipes");

        // TODO: Once we have recipes defined, we'll create recipe lines here
        // similar to how GatherPanel creates gather lines
    }

    public override void _ExitTree()
    {
        foreach (var line in _recipeLines)
        {
            if (line != null && IsInstanceValid(line))
            {
                line.QueueFree();
            }
        }
        _recipeLines.Clear();
    }
} 