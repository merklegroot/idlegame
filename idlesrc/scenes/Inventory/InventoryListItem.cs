using Godot;
using IdleGame.Models.Messages;

namespace IdleGame;

public partial class InventoryListItem : Button
{
	[Export]
	public string ResourceId { get; set; } = null;
	
	private TextureRect _icon;
	private Label _label;
	private Label _quantityLabel;
	private System.Action<string, int> _inventoryChangedHandler;
	
	public override void _Ready()
	{
		// Get references to UI elements
		_icon = GetNode<TextureRect>("HBoxContainer/Icon");
		_label = GetNode<Label>("HBoxContainer/Label");
		_quantityLabel = GetNode<Label>("HBoxContainer/Quantity");
		
		// Connect button pressed signal
		Pressed += OnPressed;
		
		// Update the display based on ResourceId
		if (!string.IsNullOrEmpty(ResourceId))
		{
			UpdateDisplay();
			
			// Connect to inventory changes
			_inventoryChangedHandler = (id, qty) => UpdateQuantity();
			GameState.Instance.InventoryChanged += _inventoryChangedHandler;
		}
	}
	
	public override void _ExitTree()
	{
		// Disconnect from inventory changes to prevent disposed object access
		if (_inventoryChangedHandler != null)
		{
			GameState.Instance.InventoryChanged -= _inventoryChangedHandler;
			_inventoryChangedHandler = null;
		}
	}
	
	private void OnPressed()
	{
		GameEvent.FireInventoryItemSelected(new InventoryItemSelectedMessage(ResourceId));
	}
	
	private void UpdateDisplay()
	{
		var resourceInfo = ResourceData.Instance.GetResourceById(ResourceId);
		if (resourceInfo != null)
		{
			_icon.Texture = GD.Load<Texture2D>(resourceInfo.Icon);
			_label.Text = resourceInfo.Name;
		}
		else
		{
			_label.Text = ResourceId;
		}
		
		UpdateQuantity();
	}
	
	private void UpdateQuantity()
	{
		if (!string.IsNullOrEmpty(ResourceId))
		{
			var quantity = GameState.Instance.GetResourceQuantity(ResourceId);
			_quantityLabel.Text = quantity.ToString();
		}
	}
} 
