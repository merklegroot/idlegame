using Godot;
using System;

namespace IdleGame;

public partial class Inventory : Control
{
	private Label _woodCount;
	private Label _stoneCount;
	
	public override void _Ready()
	{
		_woodCount = GetNode<Label>("VBoxContainer/Resources/WoodCount");
		_stoneCount = GetNode<Label>("VBoxContainer/Resources/StoneCount");
		
		// Connect to inventory changes
		GameState.Instance.InventoryChanged += UpdateDisplay;
		
		UpdateDisplay();
	}
	
	private void UpdateDisplay()
	{
		_woodCount.Text = $"Wood: {GameState.Instance.WoodCount}";
		_stoneCount.Text = $"Stone: {GameState.Instance.StoneCount}";
	}
} 