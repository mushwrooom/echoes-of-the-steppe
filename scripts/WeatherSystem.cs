using Godot;
using System;
using System.Collections.Generic;

public partial class WeatherSystem : Node
{
	public enum WeatherType { Sunny, Rainy, Snowy, Storm, ExtremeCold }

	[Export] public float WeatherChangeInterval = 60.0f;
	private float _timer = 0;

	private List<WeatherType> possibleTypes = new();
	public WeatherType CurrentWeather { get; private set; } = WeatherType.Sunny;

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
		CurrentWeather = Utils.GetRandomElement(possibleTypes);
		GD.Print("Weather changed to: " + CurrentWeather);
	}

	private void OnSeasonChanged(TimeManager.Season newSeason)
	{
		switch (newSeason)
		{
			case TimeManager.Season.Spring:
				possibleTypes.Clear();
				possibleTypes.Add(WeatherType.Sunny);
				possibleTypes.Add(WeatherType.Snowy);
				possibleTypes.Add(WeatherType.Rainy);
    			possibleTypes.Add(WeatherType.ExtremeCold);
				break;
			case TimeManager.Season.Summer:
				possibleTypes.Clear();
				possibleTypes.Add(WeatherType.Sunny);
				possibleTypes.Add(WeatherType.Rainy);
				break;
			case TimeManager.Season.Fall:
				possibleTypes.Clear();
    			possibleTypes.Add(WeatherType.Rainy);
    			possibleTypes.Add(WeatherType.Snowy);
    			possibleTypes.Add(WeatherType.ExtremeCold);
				break;
			case TimeManager.Season.Winter:
				possibleTypes.Clear();
    			possibleTypes.Add(WeatherType.Snowy);
    			possibleTypes.Add(WeatherType.ExtremeCold);
				break;
		}
	}
}
