using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace IdleGame;

public partial class ResourceData : Node
{
	public static ResourceData Instance { get; private set; }

	private Dictionary<string, ResourceInfo> _resources = new();

	public override void _EnterTree()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}

		Instance = this;

		LoadResources();
	}

	private static string ReadText<TData>(string path)
	{
		var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);

		if (file == null)
			throw new ApplicationException($"Failed to load {path}");

		try
		{
			return file.GetAsText();
		}
		finally
		{
			file.Close();
		}
	}

	private static TData ReadJson<TData>(string path)
	{
		var json = ReadText<TData>(path);
		return JsonSerializer.Deserialize<TData>(json);
	}

	private void LoadResources()
	{
		_resources = ReadJson<Dictionary<string, ResourceInfo>>("res://data/resources.json");
	}

	public ResourceInfo GetResourceInfo(string resourceId)
	{
		if (_resources.TryGetValue(resourceId.ToLower(), out var info))
		{
			return info;
		}

		GD.PrintErr($"Resource not found: {resourceId}");
		return null;
	}
}
