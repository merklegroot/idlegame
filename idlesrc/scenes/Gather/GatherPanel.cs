using Godot;
using System;
using System.Globalization;

namespace IdleGame;

public partial class GatherPanel : Control
{
    private VBoxContainer _container;

    public override void _Ready()
    {
        _container = GetNode<VBoxContainer>("VBoxContainer");
        var gatherLinePath = "res://scenes/Gather/GatherLine.tscn";
        var gatherLineScene = GD.Load<PackedScene>(gatherLinePath);

        // Remove any existing gather lines (the ones we had in scene editor)
        foreach (var child in _container.GetChildren())
        {
            if (child.Name.ToString().EndsWith("Gather"))
            {
                child.QueueFree();
            }
        }

        // Add gather lines for each resource
        foreach (var resource in ResourceData.Instance.ListResources())
        {
            var gatherLine = gatherLineScene.Instantiate<GatherLine>();
            gatherLine.ResourceId = resource.Id;
            _container.AddChild(gatherLine);
        }
    }
} 