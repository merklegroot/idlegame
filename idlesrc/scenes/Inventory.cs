using Godot;
using System;

namespace IdleGame;

public partial class Inventory : Control
{
	private Label _woodCount;
	private Label _rockCount;
	
	public override void _Ready()
	{
		_woodCount = GetNode<Label>("VBoxContainer/Resources/WoodCount");
		_rockCount = GetNode<Label>("VBoxContainer/Resources/RockCount");
		
		// Connect to inventory changes
		GameState.Instance.InventoryChanged += UpdateDisplay;
		
		UpdateDisplay();
	}
	
	private void UpdateDisplay()
	{
		_woodCount.Text = $"Wood: {GameState.Instance.WoodCount}";
		_rockCount.Text = $"Rock: {GameState.Instance.RockCount}";
	}
} 