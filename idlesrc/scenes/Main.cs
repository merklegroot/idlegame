using Godot;

namespace IdleGame;

public partial class Main : Control
{
    private Label _moneyLabel;

    public override void _Ready()
    {
        _moneyLabel = GetNode<Label>("Footer/MoneyLabel");
        GameEvent.MoneyChanged += OnMoneyChanged;
        
        OnMoneyChanged();
    }

    private void OnMoneyChanged() =>
        _moneyLabel.Text = $"Money: {GameState.Instance.GetMoney()}g";
    
    public override void _ExitTree() =>
        GameEvent.MoneyChanged -= OnMoneyChanged;
}