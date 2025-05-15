using Godot;
using System;

namespace IdleGame;

public partial class Gather : HBoxContainer
{
	[Export]
	public string ResourceName { get; set; } = "Resource";
	
	private ProgressBar _progressBar;
	private TextureRect _icon;
	private Button _gatherArea;
	private Label _countLabel;
	
	private bool _gathering = false;
	private float _gatherSpeed = 0.5f;  // Time in seconds to complete gathering
	private float _progress = 0.0f;
	
	private ResourceInfo _resourceInfo;
	
	public override void _Ready()
	{
		// Get references to UI elements
		_progressBar = GetNode<ProgressBar>("GatherArea/HBoxContainer/ProgressBar");
		_icon = GetNode<TextureRect>("GatherArea/HBoxContainer/Icon");
		_gatherArea = GetNode<Button>("GatherArea");
		_countLabel = GetNode<Label>("Count");
		
		// Get resource info
		_resourceInfo = ResourceData.Instance.GetResourceInfo(ResourceName);
		if (_resourceInfo == null)
		{
			GD.PrintErr($"Failed to load resource info for {ResourceName}");
			return;
		}
		
		// Connect button press signal
		_gatherArea.Pressed += OnGatherAreaPressed;
		
		// Connect to inventory changes
		GameState.Instance.InventoryChanged += UpdateResourceCountDisplay;
		
		// Initialize UI
		_progressBar.Value = _progress;
		_icon.Texture = GD.Load<Texture2D>(_resourceInfo.Icon);
		UpdateResourceCountDisplay();
		
		// Set tooltip
		_gatherArea.TooltipText = _resourceInfo.Description;
	}

	public override void _Process(double delta)
	{
		if (_gathering)
		{
			_progress += (float)delta / _gatherSpeed;
			_progressBar.Value = _progress;
			
			if (_progress >= 1.0f)
			{
				OnGatheringComplete();
			}
		}
	}
	
	private void OnGatherAreaPressed()
	{
		if (!_gathering)
		{
			_gathering = true;
			_progress = 0.0f;
			_progressBar.Value = _progress;
		}
	}
	
	private void OnGatheringComplete()
	{
		_gathering = false;
		_progress = 0.0f;
		_progressBar.Value = _progress;
		GameState.Instance.AddResource(ResourceName);
		GD.Print($"{_resourceInfo.Name} gathered!");
	}
	
	private void UpdateResourceCountDisplay()
	{
		var count = ResourceName.ToLower() switch
		{
			"wood" => GameState.Instance.WoodCount,
			"stone" => GameState.Instance.StoneCount,
			_ => 0
		};
		_countLabel.Text = $"{_resourceInfo.Name}: {count}";
	}
} 
