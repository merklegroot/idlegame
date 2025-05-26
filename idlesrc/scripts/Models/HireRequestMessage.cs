using System;

namespace IdleGame.Models;

public record HireRequestMessage(string ResourceId)
{
    public string Id { get; init;} = Guid.NewGuid().ToString();
}