using System;

namespace IdleGame.Models.Messages;

public record SellRequestMessage(string ResourceId, int Quantity)
{
    public string Id { get; init;} = Guid.NewGuid().ToString();
}