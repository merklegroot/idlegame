using Godot;
using System;
using System.Collections.Generic;

namespace IdleGame;

public partial class GameState : Node
{
	public static GameState Instance { get; private set; }
	
	[Signal]
	public delegate void InventoryChangedEventHandler();
	
	private int _woodCount = 0;
	private int _stoneCount = 0;
	private decimal _money = 0;
	
	public int WoodCount => _woodCount;
	public int StoneCount => _stoneCount;
	public decimal Money => _money;
	
	private static readonly Dictionary<string, decimal> SellPrices = new()
	{
		["wood"] = 2.0M,   // 2 coins per wood
		["stone"] = 3.0M,  // 3 coins per stone
	};
	
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
			case "stone":
				_stoneCount += amount;
				break;
		}
		EmitSignal(SignalName.InventoryChanged);
	}
	
	public bool SellResource(string resourceName, int amount = 1)
	{
		bool success = false;
		switch (resourceName.ToLower())
		{
			case "wood" when _woodCount >= amount:
				_woodCount -= amount;
				success = true;
				break;
			case "stone" when _stoneCount >= amount:
				_stoneCount -= amount;
				success = true;
				break;
		}
		
		if (success && SellPrices.TryGetValue(resourceName.ToLower(), out decimal price))
		{
			_money += price * amount;
			EmitSignal(SignalName.InventoryChanged);
		}
		
		return success;
	}
} 
