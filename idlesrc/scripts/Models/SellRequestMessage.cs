using System;

namespace IdleGame.Models;

public record SellRequestMessage(string ResourceId, int Quantity)
{
    public string Id { get; init;} = Guid.NewGuid().ToString();
}