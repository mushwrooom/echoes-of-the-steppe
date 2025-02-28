using Godot;
using System;
using System.Collections.Generic;
using System.Runtime;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();
	[Signal]
	public delegate void EndGameEventHandler();
	[Export] public Label DayLabel;
	[Export] public Label TimeLabel;
	[Export] public Label SeasonLabel;
	[Export] public Label WeatherLabel;
	[Export] public Label SheepCountLabel;
	[Export] public VBoxContainer Prompt;
	[Export] public Label MessageLabel;
	[Export] public Button RestartButton;
	[Export] public OptionButton CheatWeather;
	[Export] public OptionButton CheatTime;
	[Export] public OptionButton CheatSeason;
	public int sheepCount = 0;

	private TimeManager timeManager;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		UpdateLabels();
	}

	public void UpdateLabels()
	{
		timeManager = Utils.Instance.TimeManager;
		DayLabel.Text = "Day: " + timeManager.CurrentDay;
		TimeLabel.Text = "Time: " + Math.Floor(6 + (24 * timeManager._currentTime / timeManager.DayLength)) % 24
						  + ":00 (" + timeManager.CurrentTimeOfDay + ")";
		SeasonLabel.Text = "Season: " + timeManager.CurrentSeason;
		WeatherLabel.Text = "Weather: " + Utils.Instance.WeatherSystem.CurrentWeather;
		SheepCountLabel.Text = sheepCount + " sheeps";
	}

	public void AddSheepCount(int amount)
	{
		sheepCount += amount;
	}

	public void ShowGameWon()
	{
		MessageLabel.Text = "You Won!";
		Prompt.Visible = true;
	}

	public void ShowGameOver()
	{
		MessageLabel.Text = "You Lost!";
		Prompt.Visible = true;
	}
	private void OnRestartButtonPressed()
	{
		EmitSignal(SignalName.EndGame);
	}

	private void CheatWeatherSelected(int index)
	{
		Utils.Instance.WeatherSystem.CurrentWeather = (WeatherSystem.WeatherType)index;
		Utils.Instance.WeatherSystem.ApplyWeather();
	}
	private void CheatSeasonSelected(int index)
	{
		Utils.Instance.TimeManager.CurrentSeason = (TimeManager.Season)index;
	}
	private void CheatTimeSelected(int index)
	{
		Utils.Instance.TimeManager.CurrentTimeOfDay = (TimeManager.TimeOfDay)index;
	}
}
