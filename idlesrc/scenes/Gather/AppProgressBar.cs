using Godot;

namespace IdleGame;

public partial class AppProgressBar : Control
{
	private float _value = 0.0f;
	private float _maxValue = 1.0f;
	private Label _label;
	private ColorRect _background;
	private ColorRect _fill;
	private ColorRect _border;

	public float Value
	{
		get => _value;
		set
		{
			_value = Mathf.Clamp(value, 0, _maxValue);
			QueueRedraw();
		}
	}

	public float MaxValue
	{
		get => _maxValue;
		set
		{
			_maxValue = value;
			QueueRedraw();
		}
	}

	public override void _Ready()
	{
		// Create background
		_background = new ColorRect
		{
			Color = new Color(0.2f, 0.2f, 0.2f),
			MouseFilter = MouseFilterEnum.Ignore
		};
		AddChild(_background);

		// Create fill
		_fill = new ColorRect
		{
			Color = new Color(0.4f, 0.6f, 0.8f),
			MouseFilter = MouseFilterEnum.Ignore
		};
		AddChild(_fill);

		// Create border
		_border = new ColorRect
		{
			Color = new Color(0.8f, 0.8f, 0.8f),
			MouseFilter = MouseFilterEnum.Ignore
		};
		AddChild(_border);

		// Create label
		_label = new Label
		{
			Text = "Testing",
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			MouseFilter = MouseFilterEnum.Ignore
		};
		AddChild(_label);
	}

	public override void _Process(double delta)
	{
		// Update layout
		var rect = GetRect();
		var borderSize = 2;
		
		// Background fills entire control
		_background.Position = Vector2.Zero;
		_background.Size = rect.Size;

		// Border is 2px around the edges
		_border.Position = Vector2.Zero;
		_border.Size = rect.Size;
		_border.Modulate = new Color(0.8f, 0.8f, 0.8f, 1.0f);

		// Fill is based on value
		var fillWidth = Mathf.Floor(rect.Size.X * (_value / _maxValue));
		_fill.Position = new Vector2(borderSize, borderSize);
		_fill.Size = new Vector2(fillWidth - borderSize * 2, rect.Size.Y - borderSize * 2);

		// Label is centered
		_label.Position = Vector2.Zero;
		_label.Size = rect.Size;
		
		_label.Text = $"{_value}/{_maxValue}";
	}
} 
