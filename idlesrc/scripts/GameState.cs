using Godot;
using System;

namespace IdleGame;

public partial class GameState : Node
{
	public static GameState Instance { get; private set; }
	
	[Signal]
	public delegate void InventoryChangedEventHandler();
	
	private int _woodCount = 0;
	private int _rockCount = 0;
	
	public int WoodCount => _woodCount;
	public int RockCount => _rockCount;
	
	public override void _EnterTree()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		Instance = this;
	}
	
	public void AddResource(string resourceName, int amount = 1)
	{
		switch (resourceName.ToLower())
		{
			case "wood":
				_woodCount += amount;
				break;
			case "rock":
				_rockCount += amount;
				break;
		}
		EmitSignal(SignalName.InventoryChanged);
	}
} 
