using System;

namespace IdleGame.Models;

public record ResourceRequestModel(string ResourceId)
{
    public string Id { get; init;} = Guid.NewGuid().ToString();
}