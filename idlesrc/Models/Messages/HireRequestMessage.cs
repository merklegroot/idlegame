using System;

namespace IdleGame.Models.Messages;

public record HireRequestMessage(string ResourceId)
{
    public string Id { get; init;} = Guid.NewGuid().ToString();
}