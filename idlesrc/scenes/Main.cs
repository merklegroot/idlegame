using Godot;
using System;

public partial class Main : Node2D
{
	private ProgressBar _woodProgressBar;
	private ProgressBar _rockProgressBar;
	private Button _woodGatherArea;
	private Button _rockGatherArea;
	private Label _woodCountLabel;
	private Label _rockCountLabel;
	
	private bool _gatheringWood = false;
	private bool _gatheringRock = false;
	private float _gatherSpeed = 0.5f;  // Time in seconds to complete gathering
	private float _woodProgress = 0.0f;
	private float _rockProgress = 0.0f;
	private int _woodCount = 0;
	private int _rockCount = 0;
	
	public override void _Ready()
	{
		GD.Print("Hello from C# Godot!");
		
		// Get references to UI elements
		_woodProgressBar = GetNode<ProgressBar>("UI/VBoxContainer/WoodContainer/GatherArea/HBoxContainer/ProgressBar");
		_rockProgressBar = GetNode<ProgressBar>("UI/VBoxContainer/RockContainer/GatherArea/HBoxContainer/ProgressBar");
		_woodGatherArea = GetNode<Button>("UI/VBoxContainer/WoodContainer/GatherArea");
		_rockGatherArea = GetNode<Button>("UI/VBoxContainer/RockContainer/GatherArea");
		_woodCountLabel = GetNode<Label>("UI/VBoxContainer/WoodContainer/WoodCount");
		_rockCountLabel = GetNode<Label>("UI/VBoxContainer/RockContainer/RockCount");
		
		// Connect button press signals
		_woodGatherArea.Pressed += () => OnWoodButtonPressed();
		_rockGatherArea.Pressed += () => OnRockButtonPressed();
		
		// Initialize UI
		_woodProgressBar.Value = _woodProgress;
		_rockProgressBar.Value = _rockProgress;
		UpdateResourceCountDisplay();
	}

	public override void _Process(double delta)
	{
		if (_gatheringWood)
		{
			_woodProgress += (float)delta / _gatherSpeed;
			_woodProgressBar.Value = _woodProgress;
			
			if (_woodProgress >= 1.0f)
			{
				OnWoodGatheringComplete();
			}
		}
		
		if (_gatheringRock)
		{
			_rockProgress += (float)delta / _gatherSpeed;
			_rockProgressBar.Value = _rockProgress;
			
			if (_rockProgress >= 1.0f)
			{
				OnRockGatheringComplete();
			}
		}
	}
	
	private void OnWoodButtonPressed()
	{
		if (!_gatheringWood)
		{
			_gatheringWood = true;
			_woodProgress = 0.0f;
			_woodProgressBar.Value = _woodProgress;
		}
	}
	
	private void OnRockButtonPressed()
	{
		if (!_gatheringRock)
		{
			_gatheringRock = true;
			_rockProgress = 0.0f;
			_rockProgressBar.Value = _rockProgress;
		}
	}
	
	private void OnWoodGatheringComplete()
	{
		_gatheringWood = false;
		_woodProgress = 0.0f;
		_woodProgressBar.Value = _woodProgress;
		_woodCount++;
		UpdateResourceCountDisplay();
		GD.Print("Wood gathered!");
	}
	
	private void OnRockGatheringComplete()
	{
		_gatheringRock = false;
		_rockProgress = 0.0f;
		_rockProgressBar.Value = _rockProgress;
		_rockCount++;
		UpdateResourceCountDisplay();
		GD.Print("Rock gathered!");
	}
	
	private void UpdateResourceCountDisplay()
	{
		_woodCountLabel.Text = $"Wood: {_woodCount}";
		_rockCountLabel.Text = $"Rock: {_rockCount}";
	}
} 
