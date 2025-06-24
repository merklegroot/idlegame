using System;

namespace IdleGame.Models.Messages;

public record InventoryItemSelectedMessage(string ResourceId)
{
    public string Id { get; init;} = Guid.NewGuid().ToString();
} 