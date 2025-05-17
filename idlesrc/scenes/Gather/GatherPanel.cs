using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdleGame;

public partial class GatherPanel : Control
{
    private VBoxContainer _container;
    private List<GatherLine> _gatherLines = new();

    public override void _Ready()
    {
        _container = GetNode<VBoxContainer>("VBoxContainer");
        var gatherLinePath = "res://scenes/Gather/GatherLine.tscn";
        var gatherLineScene = GD.Load<PackedScene>(gatherLinePath);

        // Clean up any existing gather lines
        foreach (var line in _gatherLines)
        {
            line.QueueFree();
        }
        _gatherLines.Clear();

        // Add gather lines for each gatherable resource
        foreach (var resource in ResourceData.Instance.ListResources().Where(r => r.IsGatherable))
        {
            var gatherLine = gatherLineScene.Instantiate<GatherLine>();
            gatherLine.ResourceId = resource.Id;
            _container.AddChild(gatherLine);
            _gatherLines.Add(gatherLine);
        }
    }

    public override void _ExitTree()
    {
        // Clean up gather lines when the panel is removed
        foreach (var line in _gatherLines)
        {
            if (line != null && IsInstanceValid(line))
            {
                line.QueueFree();
            }
        }
        _gatherLines.Clear();
    }
} 