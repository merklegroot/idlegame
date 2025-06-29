using System;
using Godot;
using IdleGame.Models;
using IdleGame.Models.Messages;

namespace IdleGame;

public partial class GameEventHandler : Node
{
    public override void _Ready()
    {
        GameEvent.HireRequested += OnHireRequested;
        GameEvent.InventoryItemSelected += OnInventoryItemSelected;
        GameEvent.SellRequested += OnSellRequested;
        GameEvent.SellAllRequested += OnSellAllRequested;
    }

    public override void _ExitTree()
    {
        GameEvent.HireRequested -= OnHireRequested;
        GameEvent.InventoryItemSelected -= OnInventoryItemSelected;
        GameEvent.SellRequested -= OnSellRequested;
        GameEvent.SellAllRequested -= OnSellAllRequested;
    }

    private void OnHireRequested(HireRequestMessage request)
    {
        var resourceInfo = ResourceData.Instance.GetResourceById(request.ResourceId);
        if (resourceInfo == null)
        {
            GD.PrintErr($"Failed to load resource info for {request.ResourceId}");
            return;
        }

        var employeeCost = resourceInfo.SellPrice * 10;
        if (GameState.Instance.GetMoney() >= employeeCost)
        {
            GameState.Instance.AddMoney(-employeeCost);
            GameState.Instance.AddEmployee(request.ResourceId);
            GD.Print($"Hired an employee for {resourceInfo.Name} gathering!");
        }
    }

    private void OnInventoryItemSelected(InventoryItemSelectedMessage request)
    {
        var resourceInfo = ResourceData.Instance.GetResourceById(request.ResourceId);
        if (resourceInfo == null)
        {
            GD.PrintErr($"Failed to load resource info for {request.ResourceId}");
            return;
        }

        GD.Print($"Selected inventory item: {resourceInfo.Name}");
        // TODO: Update detail panel with selected item information
    }

    private void OnSellRequested(SellRequestMessage request)
    {
        var resourceInfo = ResourceData.Instance.GetResourceById(request.ResourceId);
        if (resourceInfo == null)
        {
            GD.PrintErr($"Failed to load resource info for {request.ResourceId}");
        }

        var quantityOnHand = GameState.Instance.GetResourceQuantity(request.ResourceId); 

        var effectiveQuantityToSell = Math.Min(quantityOnHand, request.Quantity);

        if (effectiveQuantityToSell <= 0)
        {
            return;
        }

        var totalValue = effectiveQuantityToSell * resourceInfo.SellPrice;

        GameState.Instance.AddMoney(totalValue);
        GameState.Instance.DeltaResourceQuantity(request.ResourceId, -effectiveQuantityToSell);

        // GameEvent.FireInventoryChanged(new InventoryChangedMessage(request.ResourceId, GameState.Instance.GetResourceQuantity(request.ResourceId)));
        // GameEvent.FireMoneyChanged();
    }    

    private void OnSellAllRequested(SellAllRequestMessage request)
    {
        var resourceInfo = ResourceData.Instance.GetResourceById(request.ResourceId);
        if (resourceInfo == null)
        {
            GD.PrintErr($"Failed to load resource info for {request.ResourceId}");
            return;
        }

        var quantityOnHand = GameState.Instance.GetResourceQuantity(request.ResourceId);
        if (quantityOnHand <= 0)
        {
            GD.PrintErr($"No {resourceInfo.Name} to sell");
            return;
        }

        var totalValue = quantityOnHand * resourceInfo.SellPrice;

        GameState.Instance.AddMoney(totalValue);
        GameState.Instance.DeltaResourceQuantity(request.ResourceId, -quantityOnHand);

        // GameEvent.FireInventoryChanged(new InventoryChangedMessage(request.ResourceId, 0));
        // GameEvent.FireMoneyChanged();
    }    
}