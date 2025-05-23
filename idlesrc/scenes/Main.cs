using Godot;

namespace IdleGame;

public partial class Main : Control
{
    private Label _moneyLabel;
    
    public override void _Ready()
    {
        _moneyLabel = GetNode<Label>("Footer/MoneyLabel");
        
        // Connect to money changes
        GameState.Instance.MoneyChanged += UpdateMoneyDisplay;
        
        // Initial display
        UpdateMoneyDisplay();
    }
    
    private void UpdateMoneyDisplay()
    {
        _moneyLabel.Text = $"Money: {GameState.Instance.GetMoney()}g";
    }
    
    public override void _ExitTree()
    {
        // Disconnect from money changes
        GameState.Instance.MoneyChanged -= UpdateMoneyDisplay;
    }
} 