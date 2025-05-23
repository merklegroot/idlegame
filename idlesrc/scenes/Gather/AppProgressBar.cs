using Godot;

namespace IdleGame;

public partial class AppProgressBar : ProgressBar
{
	public AppProgressBar()
	{
		// Set default values for properties we use
		MaxValue = 1.0f;
		Step = 0.01f;
		ShowPercentage = false;
		MouseFilter = MouseFilterEnum.Ignore;
	}
} 
