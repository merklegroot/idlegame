using Godot;
using System.Collections.Generic;
using System.Linq;

namespace IdleGame;

public partial class CraftPanel : Control
{
    private VBoxContainer _recipeContainer;
    private List<Control> _recipeLines = new();

    public override void _Ready()
    {
        _recipeContainer = GetNode<VBoxContainer>("VBoxContainer/Recipes");
        var craftLinePath = "res://scenes/Craft/CraftLine.tscn";
        var craftLineScene = GD.Load<PackedScene>(craftLinePath);

        // Clean up any existing recipe lines
        foreach (var line in _recipeLines)
        {
            if (line != null && IsInstanceValid(line))
            {
                line.QueueFree();
            }
        }
        _recipeLines.Clear();

        // Add craft lines for each craftable resource (resources with recipes)
        foreach (var resource in ResourceData.Instance.ListResources().Where(r => r.Recipe != null))
        {
            var craftLine = craftLineScene.Instantiate<CraftLine>();
            craftLine.ResourceId = resource.Id;
            _recipeContainer.AddChild(craftLine);
            _recipeLines.Add(craftLine);
        }
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