using Godot;
using System;
using System.Collections.Generic;

public partial class Utils : Node
{
	public static Utils Instance { get; private set; }
	[Export] public TimeManager TimeManager;
	[Export] public WeatherSystem WeatherSystem;


	private static readonly Random random = new();
	public override void _Ready()
	{
		Instance = this;
	}

	public bool IsNight()
	{
		return TimeManager.CurrentTimeOfDay == TimeManager.TimeOfDay.Night;
	}
	public static T GetRandomElement<T>(List<T> list)
    {
        if (list == null || list.Count == 0) 
            throw new ArgumentException("List cannot be null or empty.");

        return list[random.Next(list.Count)];
    }
}
