using Godot;
using System;
using IdleGame.Models;
using System.Text.Json;
namespace IdleGame;

public partial class GatherLine : VBoxContainer
{
	[Export]
	public string ResourceId { get; set; } = null;
	
	private AppProgressBar _progressBar;
	private AppProgressBar _employeeProgressBar;
	private TextureRect _icon;
	private Button _gatherArea;
	private Label _countLabel;
	private Label _employeeCountLabel;
	private Button _hireButton;
	
	private bool _gathering = false;
	private float _gatherSpeed = 0.5f;  // Time in seconds to complete gathering
	private float _progress = 0.0f;
	private float _employeeProgress = 0.0f;
	private float _employeeGatherSpeed = 2.0f;  // Base time in seconds for one employee
	
	private ResourceInfo _resourceInfo;
	private float _employeeCost;
	
	// Add event for hire requests
	public static event Action<ResourceRequestModel> HireRequested;
	
	public override void _Ready()
	{
		// Get references to UI elements
		_progressBar = GetNode<AppProgressBar>("MainInfo/GatherArea/HBoxContainer/ProgressBar");
		_employeeProgressBar = GetNode<AppProgressBar>("EmployeeInfo/EmployeeProgress");
		_icon = GetNode<TextureRect>("MainInfo/Icon");
		_gatherArea = GetNode<Button>("MainInfo/GatherArea");
		_countLabel = GetNode<Label>("MainInfo/Count");
		_employeeCountLabel = GetNode<Label>("EmployeeInfo/EmployeeCount");
		_hireButton = GetNode<Button>("EmployeeInfo/HireButton");
		
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
		
		// Handle employee gathering
		var employeeCount = GameState.Instance.GetEmployeeCount(ResourceId);
		if (employeeCount > 0)
		{
			_employeeProgress += (float)delta * employeeCount / _employeeGatherSpeed;
			_employeeProgressBar.Value = _employeeProgress;
			
			if (_employeeProgress >= 1.0f)
			{
				OnEmployeeGatheringComplete();
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
	
	private void OnHireButtonPressed()
	{
		var requestMessage = new ResourceRequestModel(ResourceId);

		// Fire the hire event
		HireRequested?.Invoke(requestMessage);
		GD.Print($"Hire requested for resource: {JsonSerializer.Serialize(requestMessage)}");
	}
	
	private void OnGatheringComplete()
	{
		_gathering = false;
		_progress = 0.0f;
		_progressBar.Value = _progress;
		GameState.Instance.AddResource(ResourceId);
		GD.Print($"{_resourceInfo.Name} gathered!");
	}
	
	private void OnEmployeeGatheringComplete()
	{
		_employeeProgress = 0.0f;
		_employeeProgressBar.Value = _employeeProgress;
		GameState.Instance.AddResource(ResourceId);
		GD.Print($"{_resourceInfo.Name} gathered by employees!");
	}
	
	private void UpdateResourceCountDisplay()
	{
		var count = GameState.Instance.GetResouceQuantity(ResourceId);
		_countLabel.Text = $"{_resourceInfo?.Name}: {count}";
	}
	
	private void UpdateEmployeeDisplay()
	{
		var count = GameState.Instance.GetEmployeeCount(ResourceId);
		_employeeCountLabel.Text = $"Employees: {count}";
		_hireButton.Text = $"Hire ({_employeeCost:F1}g)";
		_hireButton.Disabled = GameState.Instance.GetMoney() < _employeeCost;
	}
} 
