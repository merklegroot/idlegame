using Godot;
using System.Linq;

namespace IdleGame;

public partial class CraftLine : HBoxContainer
{
	[Export]
	public string ResourceId { get; set; } = null;
	
	private TextureRect _icon;
	private Button _craftArea;
	private Label _nameLabel;
	private Label _quantityLabel;
	private Label _requirementsLabel;
	
	private ResourceInfo _resourceInfo;
	
	public override void _Ready()
	{
		// Get references to UI elements
		_icon = GetNode<TextureRect>("CraftArea/HBoxContainer/Icon");
		_craftArea = GetNode<Button>("CraftArea");
		_nameLabel = GetNode<Label>("CraftArea/HBoxContainer/Label");
		_quantityLabel = GetNode<Label>("CraftArea/HBoxContainer/Quantity");
		_requirementsLabel = GetNode<Label>("Requirements");
		
		// Get resource info
		_resourceInfo = ResourceData.Instance.GetResourceById(ResourceId);
		if (_resourceInfo == null)
		{
			GD.PrintErr($"Failed to load resource info for {ResourceId}");
			return;
		}
		
		// Connect button press signal
		_craftArea.Pressed += OnCraftAreaPressed;
		
		// Connect to inventory changes to update requirements display
		GameState.Instance.InventoryChanged += (id, qty) => UpdateDisplay();
		
		// Initialize UI
		_icon.Texture = GD.Load<Texture2D>(_resourceInfo.Icon);
		_nameLabel.Text = _resourceInfo.Name;
		_craftArea.TooltipText = _resourceInfo.Description;
		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		// Update crafted item quantity
		var craftedQuantity = GameState.Instance.GetResouceQuantity(ResourceId);
		_quantityLabel.Text = $"({craftedQuantity})";
		
		// Update requirements
		var requirements = "";
		var canCraft = true;
		
		foreach (var ingredient in _resourceInfo.Recipe)
		{
			var ingredientInfo = ResourceData.Instance.GetResourceById(ingredient.Id);
			var currentQuantity = GameState.Instance.GetResouceQuantity(ingredient.Id);
			var hasEnough = currentQuantity >= ingredient.Quantity;
			
			requirements += $"{ingredientInfo.Name}: {currentQuantity}/{ingredient.Quantity}";
			if (!hasEnough)
			{
				requirements += " (Not Enough!)";
				canCraft = false;
			}
			requirements += "\n";
		}
		
		_requirementsLabel.Text = requirements.TrimEnd('\n');
		_craftArea.Disabled = !canCraft;
	}
	
	private void OnCraftAreaPressed()
	{
		// Verify we have all ingredients
		if (!_resourceInfo.Recipe.All(ingredient => 
			GameState.Instance.GetResouceQuantity(ingredient.Id) >= ingredient.Quantity))
		{
			return;
		}
		
		// Remove ingredients
		foreach (var ingredient in _resourceInfo.Recipe)
		{
			GameState.Instance.AddResource(ingredient.Id, -ingredient.Quantity);
		}
		
		// Add crafted item
		GameState.Instance.AddResource(ResourceId);
		GD.Print($"{_resourceInfo.Name} crafted!");
	}
}
