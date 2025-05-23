using Godot;
using System;

namespace IdleGame;

public partial class GatherLine : VBoxContainer
{
	[Export]
	public string ResourceId { get; set; } = null;
	
	private ProgressBar _progressBar;
	private ProgressBar _employeeProgressBar;
	private TextureRect _icon;
	private Button _gatherArea;
	private Label _countLabel;
	private Label _employeeCountLabel;
	private Button _hireButton;
	
	private bool _gathering = false;
	private float _gatherSpeed = 0.5f;  // Time in seconds to complete gathering
	private float _progress = 0.0f;
	private float _employeeProgress = 0.0f;
	
	private ResourceInfo _resourceInfo;
	private float _employeeCost;
	
	public override void _Ready()
	{
		// Get references to UI elements
		_progressBar = GetNode<ProgressBar>("MainInfo/GatherArea/HBoxContainer/ProgressBar");
		_employeeProgressBar = GetNode<ProgressBar>("EmployeeInfo/EmployeeProgress");
		_icon = GetNode<TextureRect>("MainInfo/Icon");
		_gatherArea = GetNode<Button>("MainInfo/GatherArea");
		_countLabel = GetNode<Label>("MainInfo/Count");
		_employeeCountLabel = GetNode<Label>("EmployeeInfo/EmployeeCount");
		_hireButton = GetNode<Button>("HireButton");
		
		// Get resource info
		_resourceInfo = ResourceData.Instance.GetResourceById(ResourceId);
		if (_resourceInfo == null)
		{
			GD.PrintErr($"Failed to load resource info for {ResourceId}");
			return;
		}
		
		// Calculate employee cost (10x sell price)
		_employeeCost = _resourceInfo.SellPrice * 10;
		
		// Connect button press signals
		_gatherArea.Pressed += OnGatherAreaPressed;
		_hireButton.Pressed += OnHireButtonPressed;
		
		// Connect to inventory changes
		GameState.Instance.InventoryChanged += (id, qty) => UpdateResourceCountDisplay();
		GameState.Instance.EmployeesChanged += (id, count) => UpdateEmployeeDisplay();
		GameState.Instance.MoneyChanged += UpdateEmployeeDisplay;
		
		// Initialize UI
		_progressBar.Value = _progress;
		_employeeProgressBar.Value = _employeeProgress;
		_icon.Texture = GD.Load<Texture2D>(_resourceInfo.Icon);
		UpdateResourceCountDisplay();
		UpdateEmployeeDisplay();
		
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
		GameState.Instance.AddResource(ResourceId);
		GD.Print($"{_resourceInfo.Name} gathered!");
	}
	
	private void UpdateResourceCountDisplay()
	{
		var count = GameState.Instance.GetResouceQuantity(ResourceId);
		
		_countLabel.Text = $"{_resourceInfo?.Name}: {count}";
	}
	
	private void OnHireButtonPressed()
	{
		if (GameState.Instance.GetMoney() >= _employeeCost)
		{
			GameState.Instance.AddMoney(-_employeeCost);
			GameState.Instance.AddEmployee(ResourceId);
			GD.Print($"Hired an employee for {_resourceInfo.Name} gathering!");
		}
	}
	
	private void UpdateEmployeeDisplay()
	{
		var count = GameState.Instance.GetEmployeeCount(ResourceId);
		_employeeCountLabel.Text = $"Employees: {count}";
		_hireButton.Text = $"Hire ({_employeeCost:F1}g)";
		_hireButton.Disabled = GameState.Instance.GetMoney() < _employeeCost;
	}
} 
