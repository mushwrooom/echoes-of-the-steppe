using Godot;
using System;

public partial class Utils : Node
{
	public static Utils Instance { get; private set; }
	TimeManager timeManager;
	public override void _Ready()
	{
		Instance = this;
		timeManager = GetNode<TimeManager>("/root/Game/TimeManager");
	}

	public bool IsNight()
	{
		return timeManager.CurrentTimeOfDay == TimeManager.TimeOfDay.Night;
	}
}
