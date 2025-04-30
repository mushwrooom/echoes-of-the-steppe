using Godot;
using System;
using System.Collections.Generic;

public partial class WeatherSystem : Node
{
	public enum WeatherType { Sunny, Rainy, Snowy, Storm, ExtremeCold }

	[Export] private CpuParticles2D RainParticle;
	[Export] private CpuParticles2D SnowParticle;
	public float WeatherChangeInterval = 60.0f;
	private float _timer = 0;

	private List<WeatherType> possibleTypes = new();
	public WeatherType CurrentWeather { get; set; } = WeatherType.ExtremeCold;

	[Signal] public delegate void WeatherChangedEventHandler(WeatherType newWeather);
	public override void _Ready()
	{
		ApplyWeather();
	}
	public override void _Process(double delta)
	{
		_timer += (float)delta;
		if (_timer >= WeatherChangeInterval)
		{
			_timer = 0;
			ChangeWeather();
		}
	}
	public void ApplyWeather()
	{
		switch (CurrentWeather)
		{
			case WeatherType.Snowy:
				RainParticle.Emitting = false;
				SnowParticle.Amount = 40;
				SnowParticle.Gravity = new Vector2(100, 480);
				SnowParticle.Emitting = true;
				break;
			case WeatherType.Rainy:
				SnowParticle.Emitting = false;
				RainParticle.Amount = 70;
				RainParticle.Emitting = true;
				break;
			case WeatherType.Storm:
				SnowParticle.Emitting = false;
				RainParticle.Amount = 140;
				RainParticle.Emitting = true;
				break;
			case WeatherType.Sunny:
				SnowParticle.Emitting = false;
				RainParticle.Emitting = false;
				break;
			case WeatherType.ExtremeCold:
				RainParticle.Emitting = false;
				SnowParticle.Amount = 100;
				SnowParticle.Gravity = new Vector2(200,480);
				SnowParticle.Emitting = true;
				break;
		}
	}
	public void ChangeWeather()
	{
		CurrentWeather = Utils.GetRandomElement(possibleTypes);
		ApplyWeather();
		GD.Print("Weather changed to: " + CurrentWeather);
	}

	public void OnSeasonChanged(TimeManager.Season newSeason)
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
