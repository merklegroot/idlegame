using Godot;
using System;

namespace IdleGame;

public partial class Gather : HBoxContainer
{
	private static readonly Texture2D WoodIcon = GD.Load<Texture2D>("res://assets/rpg-icons/Material/Wood Log.png");
	private static readonly Texture2D RockIcon = GD.Load<Texture2D>("res://assets/cute-fantasy/individual_tiles/outdoor_decor/ground_rocks.png");
	
	[Export]
	public string ResourceName { get; set; } = "Resource";
	
	private ProgressBar _progressBar;
	private TextureRect _icon;
	private Button _gatherArea;
	private Label _countLabel;
	
	private bool _gathering = false;
	private float _gatherSpeed = 0.5f;  // Time in seconds to complete gathering
	private float _progress = 0.0f;
	
	public override void _Ready()
	{
		// Get references to UI elements
		_progressBar = GetNode<ProgressBar>("GatherArea/HBoxContainer/ProgressBar");
		_icon = GetNode<TextureRect>("GatherArea/HBoxContainer/Icon");
		_gatherArea = GetNode<Button>("GatherArea");
		_countLabel = GetNode<Label>("Count");
		
		// Connect button press signal
		_gatherArea.Pressed += OnGatherAreaPressed;
		
		// Connect to inventory changes
		GameState.Instance.InventoryChanged += UpdateResourceCountDisplay;
		
		// Initialize UI
		_progressBar.Value = _progress;
		_icon.Texture = ResourceName.ToLower() switch
		{
			"wood" => WoodIcon,
			"rock" => RockIcon,
			_ => null
		};
		UpdateResourceCountDisplay();
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
		GD.Print($"{ResourceName} gathered!");
	}
	
	private void UpdateResourceCountDisplay()
	{
		var count = ResourceName.ToLower() switch
		{
			"wood" => GameState.Instance.WoodCount,
			"rock" => GameState.Instance.RockCount,
			_ => 0
		};
		_countLabel.Text = $"{ResourceName}: {count}";
	}
} 