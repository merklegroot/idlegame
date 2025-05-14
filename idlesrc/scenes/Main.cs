using Godot;
using System;

public partial class Main : Node2D
{
	private ProgressBar _progressBar;
	private TextureButton _woodButton;
	
	private bool _gathering = false;
	private float _gatherSpeed = 0.5f;  // Time in seconds to complete gathering
	private float _progress = 0.0f;
	
	public override void _Ready()
	{
		GD.Print("Hello from C# Godot!");
		
		// Get references to UI elements
		_progressBar = GetNode<ProgressBar>("UI/HBoxContainer/ProgressBar");
		_woodButton = GetNode<TextureButton>("UI/HBoxContainer/WoodButton");
		
		// Connect button press signal
		_woodButton.Pressed += OnWoodButtonPressed;
		
		// Initialize progress bar
		_progressBar.Value = _progress;
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
		GD.Print("Wood gathered!");
	}
} 
