using Godot;

namespace IdleGame;

public partial class InventoryLine : HBoxContainer
{
	[Export]
	public string ResourceId { get; set; } = null;
	
	private TextureRect _icon;
	private Label _nameLabel;
	private Label _quantityLabel;
	
	private ResourceInfo _resourceInfo;
	
	public override void _Ready()
	{
		// Get references to UI elements
		_icon = GetNode<TextureRect>("Icon");
		_nameLabel = GetNode<Label>("Label");
		_quantityLabel = GetNode<Label>("Quantity");
		
		// Get resource info
		_resourceInfo = ResourceData.Instance.GetResourceById(ResourceId);
		if (_resourceInfo == null)
		{
			GD.PrintErr($"Failed to load resource info for {ResourceId}");
			return;
		}
		
		// Connect to inventory changes
		GameState.Instance.InventoryChanged += UpdateDisplay;
		
		// Initialize UI
		_icon.Texture = GD.Load<Texture2D>(_resourceInfo.Icon);
		_nameLabel.Text = _resourceInfo.Name;
		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		var quantity = GameState.Instance.GetResouceQuantity(ResourceId);
		_quantityLabel.Text = quantity.ToString();
	}
} 