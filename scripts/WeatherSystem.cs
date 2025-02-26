using Godot;
using System;

public partial class WeatherSystem : Node
{
	public enum WeatherType { Sunny, Rainy, Snowy, Storm, ExtremeCold }

	[Export] public float WeatherChangeInterval = 60.0f; // Change weather every minute
	private float _timer = 0;

	public WeatherType CurrentWeather { get; private set; } = WeatherType.Sunny;
	private Random _random = new Random();

	[Signal] public delegate void WeatherChangedEventHandler(WeatherType newWeather);

	public override void _Process(double delta)
	{
		_timer += (float)delta;
		if (_timer >= WeatherChangeInterval)
		{
			_timer = 0;
			ChangeWeather();
		}
	}

	private void ChangeWeather()
	{
		CurrentWeather = (WeatherType)_random.Next(0, Enum.GetValues(typeof(WeatherType)).Length);
		GD.Print("Weather changed to: " + CurrentWeather);
	}

	private void OnSeasonChanged(TimeManager.Season newSeason)
	{
		switch (newSeason)
		{
			case TimeManager.Season.Spring:
				// More rain, mild temperature
				WeatherChangeInterval = 45.0f;
				break;
			case TimeManager.Season.Summer:
				// Mostly sunny, sometimes storms
				WeatherChangeInterval = 60.0f;
				break;
			case TimeManager.Season.Fall:
				// Increasing cold, mix of rain and sun
				WeatherChangeInterval = 50.0f;
				break;
			case TimeManager.Season.Winter:
				// Heavy snow, extreme cold
				WeatherChangeInterval = 30.0f;
				break;
		}
	}
}
