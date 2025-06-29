using System;

namespace IdleGame.Models.Messages;

public record SellAllRequestMessage(string ResourceId)
{
    public string Id { get; init;} = Guid.NewGuid().ToString();
}