using Godot;

namespace IdleGame;

public partial class InventoryLine : VBoxContainer
{
	[Export]
	public string ResourceId { get; set; } = null;
	
	private TextureRect _icon;
	private Label _nameLabel;
	private Label _quantityLabel;
	private Label _priceLabel;
	private Button _sellOneButton;
	private Button _sellAllButton;
	
	private ResourceInfo _resourceInfo;
	
	public override void _Ready()
	{
		// Get references to UI elements
		_icon = GetNode<TextureRect>("MainInfo/Icon");
		_nameLabel = GetNode<Label>("MainInfo/Label");
		_quantityLabel = GetNode<Label>("MainInfo/Quantity");
		_priceLabel = GetNode<Label>("Price");
		_sellOneButton = GetNode<Button>("ButtonContainer/SellOne");
		_sellAllButton = GetNode<Button>("ButtonContainer/SellAll");
		
		// Get resource info
		_resourceInfo = ResourceData.Instance.GetResourceById(ResourceId);
		if (_resourceInfo == null)
		{
			GD.PrintErr($"Failed to load resource info for {ResourceId}");
			return;
		}
		
		// Connect to inventory changes
		GameState.Instance.InventoryChanged += (id, qty) => UpdateDisplay();
		
		// Connect button signals
		_sellOneButton.Pressed += OnSellOnePressed;
		_sellAllButton.Pressed += OnSellAllPressed;
		
		// Initialize UI
		_icon.Texture = GD.Load<Texture2D>(_resourceInfo.Icon);
		_nameLabel.Text = _resourceInfo.Name;
		_priceLabel.Text = $"Sell Price: {_resourceInfo.SellPrice}g";
		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		var quantity = GameState.Instance.GetResouceQuantity(ResourceId);
		_quantityLabel.Text = quantity.ToString();
		
		// Disable sell buttons if we don't have any of this resource
		_sellOneButton.Disabled = quantity < 1;
		_sellAllButton.Disabled = quantity < 1;
	}
	
	private void OnSellOnePressed()
	{
		var quantity = GameState.Instance.GetResouceQuantity(ResourceId);
		if (quantity >= 1)
		{
			// Remove one item
			GameState.Instance.AddResource(ResourceId, -1);
			// Add money
			GameState.Instance.AddMoney(_resourceInfo.SellPrice);
			GD.Print($"Sold 1 {_resourceInfo.Name} for {_resourceInfo.SellPrice}g");
		}
	}
	
	private void OnSellAllPressed()
	{
		var quantity = GameState.Instance.GetResouceQuantity(ResourceId);
		if (quantity > 0)
		{
			// Calculate total value
			var totalValue = quantity * _resourceInfo.SellPrice;
			
			// Remove all items
			GameState.Instance.AddResource(ResourceId, -quantity);
			// Add money
			GameState.Instance.AddMoney(totalValue);
			GD.Print($"Sold {quantity} {_resourceInfo.Name} for {totalValue}g");
		}
	}
} 