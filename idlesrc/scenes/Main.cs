using Godot;
using System;

public partial class Main : Node2D
{
	public override void _Ready()
	{
		GD.Print("Hello from C# Godot!");
	}

	public override void _Process(double delta)
	{
		// Called every frame. Delta is time since last frame.
	}
} 
