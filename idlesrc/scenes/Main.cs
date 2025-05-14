using Godot;
using System;

public partial class Main : Node2D
{
	private ProgressBar _progressBar;
	private TextureButton _woodButton;
	private Label _woodCountLabel;
	
	private bool _gathering = false;
	private float _gatherSpeed = 0.5f;  // Time in seconds to complete gathering
	private float _progress = 0.0f;
	private int _woodCount = 0;
	
	public override void _Ready()
	{
		GD.Print("Hello from C# Godot!");
		
		// Get references to UI elements
		_progressBar = GetNode<ProgressBar>("UI/HBoxContainer/ProgressBar");
		_woodButton = GetNode<TextureButton>("UI/HBoxContainer/WoodButton");
		_woodCountLabel = GetNode<Label>("UI/HBoxContainer/WoodCount");
		
		// Connect button press signal
		_woodButton.Pressed += OnWoodButtonPressed;
		
		// Initialize UI
		_progressBar.Value = _progress;
		UpdateWoodCountDisplay();
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
	
	private void OnWoodButtonPressed()
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
		
		_woodCount++;
		UpdateWoodCountDisplay();
		GD.Print($"Wood gathered! Total: {_woodCount}");
	}
	
	private void UpdateWoodCountDisplay()
	{
		_woodCountLabel.Text = $"Wood: {_woodCount}";
	}
} 
