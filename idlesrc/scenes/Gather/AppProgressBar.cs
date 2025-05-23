using Godot;

namespace IdleGame;

public partial class AppProgressBar : Control
{
	private float _value = 0.0f;
	private float _maxValue = 1.0f;
	private Label _label;
	private ColorRect _background;
	private ColorRect _fill;
	private ColorRect _borderTop;
	private ColorRect _borderRight;
	private ColorRect _borderBottom;
	private ColorRect _borderLeft;
	private const int BorderSize = 2;

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

		// Create border edges
		var borderColor = new Color(0.8f, 0.8f, 0.8f);

		_borderTop = new ColorRect
		{
			Color = borderColor,
			MouseFilter = MouseFilterEnum.Ignore
		};
		AddChild(_borderTop);

		_borderRight = new ColorRect
		{
			Color = borderColor,
			MouseFilter = MouseFilterEnum.Ignore
		};
		AddChild(_borderRight);

		_borderBottom = new ColorRect
		{
			Color = borderColor,
			MouseFilter = MouseFilterEnum.Ignore
		};
		AddChild(_borderBottom);

		_borderLeft = new ColorRect
		{
			Color = borderColor,
			MouseFilter = MouseFilterEnum.Ignore
		};
		AddChild(_borderLeft);

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
		
		// Background fills entire control
		_background.Position = Vector2.Zero;
		_background.Size = rect.Size;

		// Position border edges
		_borderTop.Position = Vector2.Zero;
		_borderTop.Size = new Vector2(rect.Size.X, BorderSize);

		_borderRight.Position = new Vector2(rect.Size.X - BorderSize, 0);
		_borderRight.Size = new Vector2(BorderSize, rect.Size.Y);

		_borderBottom.Position = new Vector2(0, rect.Size.Y - BorderSize);
		_borderBottom.Size = new Vector2(rect.Size.X, BorderSize);

		_borderLeft.Position = Vector2.Zero;
		_borderLeft.Size = new Vector2(BorderSize, rect.Size.Y);

		// Fill width based on value/maxValue ratio
		var fillWidth = Mathf.Floor(rect.Size.X * (_value / _maxValue));
		_fill.Position = new Vector2(BorderSize, BorderSize);
		_fill.Size = new Vector2(fillWidth - BorderSize * 2, rect.Size.Y - BorderSize * 2);

		// Label is centered
		_label.Position = Vector2.Zero;
		_label.Size = rect.Size;
		
		_label.Text = $"{_value}/{_maxValue}";
	}
} 
