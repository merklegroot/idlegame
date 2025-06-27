using Godot;
using IdleGame.Models;
using IdleGame.Models.Messages;

namespace IdleGame;

public partial class InventoryDetailsPanel : Control
{
    private TextureRect _icon;
    private Label _nameLabel;
    private Label _descriptionLabel;
    private Label _sellPriceLabel;
    private Label _quantityLabel;
    private string _selectedResourceId;
    
    public override void _Ready()
    {
        // Get references to UI elements
        _icon = GetNode<TextureRect>("Panel/VBoxContainer/Icon");
        _nameLabel = GetNode<Label>("Panel/VBoxContainer/Name");
        _descriptionLabel = GetNode<Label>("Panel/VBoxContainer/Description");
        _sellPriceLabel = GetNode<Label>("Panel/VBoxContainer/SellPrice");
        _quantityLabel = GetNode<Label>("Panel/VBoxContainer/Quantity");
        
        GameEvent.InventoryItemSelected += OnInventoryItemSelected;
        GameState.Instance.InventoryChanged += (id, qty) => UpdateQuantity();
    }
    
    public override void _ExitTree()
    {
        GameEvent.InventoryItemSelected -= OnInventoryItemSelected;
    }

    private void OnInventoryItemSelected(InventoryItemSelectedMessage message)
    {
        _selectedResourceId = message.ResourceId;
        UpdateDisplay();
        GD.Print($"InventoryDetailsPanel - OnInventoryItemSelected: {message.ResourceId}");
    }
    
    private void UpdateDisplay()
    {
        if (string.IsNullOrEmpty(_selectedResourceId))
        {
            ClearDisplay();
            return;
        }
        
        var resourceInfo = ResourceData.Instance.GetResourceById(_selectedResourceId);
        if (resourceInfo == null)
        {
            ClearDisplay();
            return;
        }
        
        // Update UI elements
        _icon.Texture = GD.Load<Texture2D>(resourceInfo.Icon);
        _nameLabel.Text = resourceInfo.Name;
        _descriptionLabel.Text = resourceInfo.Description;
        _sellPriceLabel.Text = $"Sell Price: {resourceInfo.SellPrice}g";
        UpdateQuantity();
    }
    
    private void UpdateQuantity()
    {
        if (!string.IsNullOrEmpty(_selectedResourceId))
        {
            var quantity = GameState.Instance.GetResourceQuantity(_selectedResourceId);
            _quantityLabel.Text = $"Quantity: {quantity}";
        }
    }
    
    private void ClearDisplay()
    {
        _icon.Texture = null;
        _nameLabel.Text = "No item selected";
        _descriptionLabel.Text = "";
        _sellPriceLabel.Text = "";
        _quantityLabel.Text = "";
    }
} 