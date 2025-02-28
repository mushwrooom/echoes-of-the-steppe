using Godot;
using System;

public partial class DayNightManager : CanvasModulate
{
	private PointLight2D _gerLight;
	private TimeManager _timeManager;

	private readonly Color dayCold = new("#A3D8FF");
	private readonly Color nightCold = new("#0D1B2A");
	private readonly Color MorningColor = new(1.0f, 0.9f, 0.7f, 1.0f);  // Warm sunrise
	private readonly Color AfternoonColor = new(1.0f, 1.0f, 1.0f, 1.0f); // Neutral daylight
	private readonly Color EveningColor = new(0.8f, 0.6f, 0.5f, 1.0f);  // Orange sunset
	private readonly Color NightColor = new(0.2f, 0.2f, 0.4f, 1.0f);   // Dark blue night

	private Color _targetColor;

	public override void _Ready()
	{
		_gerLight = GetNode<PointLight2D>("/root/Game/Ger/PointLight2D");
		_timeManager = GetNode<TimeManager>("/root/Game/TimeManager");

		Color = MorningColor;
		OnTimeChanged((int)_timeManager.CurrentTimeOfDay);
	}

	public override void _Process(double delta)
	{
		Color = Color.Lerp(_targetColor, 0.01f);

		if (Utils.Instance.IsNight())
			_gerLight.Energy = Mathf.Lerp(_gerLight.Energy, 1.0f, 0.01f);
		else
			_gerLight.Energy = 0;
	}

	private void OnTimeChanged(int newTime)
	{
		TimeManager.TimeOfDay time = (TimeManager.TimeOfDay)newTime;
		switch (time)
		{
			case TimeManager.TimeOfDay.Morning:
				_targetColor = MorningColor;
				break;
			case TimeManager.TimeOfDay.Afternoon:
				_targetColor = AfternoonColor;
				break;
			case TimeManager.TimeOfDay.Evening:
				_targetColor = EveningColor;
				break;
			case TimeManager.TimeOfDay.Night:
				_targetColor = NightColor;
				break;
		}
		if (Utils.Instance.WeatherSystem.CurrentWeather == WeatherSystem.WeatherType.ExtremeCold)
			if (time == TimeManager.TimeOfDay.Night)
				_targetColor = nightCold;
			else
				_targetColor = dayCold;
	}
}
