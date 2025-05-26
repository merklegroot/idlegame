using System;
using IdleGame.Models;
using IdleGame.Models.Messages;

public class GameEvent
{
    public static event Action<HireRequestMessage> HireRequested;

    public static event Action<InventoryChangedMessage> InventoryChanged;

    public static event Action<SellRequestMessage> SellRequested;

    public static event Action MoneyChanged;

    public static void FireHireRequested(HireRequestMessage request)
    {
        HireRequested?.Invoke(request);
    }

    public static void FireInventoryChanged(InventoryChangedMessage request)
    {
        InventoryChanged?.Invoke(request);
    }

    public static void FireMoneyChanged()
    {
        MoneyChanged?.Invoke();
    }

    public static void FireSellRequested(SellRequestMessage request)
    {
        SellRequested?.Invoke(request);
    }
}