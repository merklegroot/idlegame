using Godot;
using System.Collections.Generic;

namespace IdleGame;

public partial class GameState : Node
{
	public static GameState Instance { get; private set; }
	
	[Signal]
	public delegate void InventoryChangedEventHandler();
	

	private Dictionary<string, int> _resourceQuantities = new();

	public int GetResouceQuantity(string resourceId)
	{
		if(!_resourceQuantities.TryGetValue(resourceId, out int quantity))
		{
			return 0;
		}

		return quantity;
	}

	public void AddResource(string resourceId, int quantity = 1)
	{
		if(!_resourceQuantities.TryGetValue(resourceId, out int currentQuantity))
		{
			currentQuantity = 0;
		}

		_resourceQuantities[resourceId] = currentQuantity + quantity;
		EmitSignal(SignalName.InventoryChanged);
	}

	private decimal _money = 0;
	
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
	
	private Dictionary<string, int> _resources = new();
	
	public bool SellResource(string resourceName, int amount = 1)
	{
		bool success = false;
		var quantity = GetResouceQuantity(resourceName);
		
		if (quantity >= amount)
		{
			AddResource(resourceName, -amount);
			success = true;
		}
		
		if (success && SellPrices.TryGetValue(resourceName.ToLower(), out decimal price))
		{
			_money += price * amount;
			EmitSignal(SignalName.InventoryChanged);
		}
		
		return success;
	}
} 
