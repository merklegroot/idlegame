using System;
using IdleGame.Models;

public class GameEvent
{
    public static event Action<ResourceRequestModel> HireRequested;

    public static event Action<InventoryChangedMessage> InventoryChanged;

    public static event Action MoneyChanged;

    public static void FireHireRequested(ResourceRequestModel request)
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
}