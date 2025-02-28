using Godot;
using System;

public partial class TimeManager : Node
{
    public enum TimeOfDay { Morning, Afternoon, Evening, Night }
    public enum Season { Spring, Summer, Fall, Winter }

    public float DayLength = 30.0f;
    private float _runningTime = 0;
    public float _currentTime = 0;

    public int CurrentDay = 0;
    public TimeOfDay CurrentTimeOfDay { get; set; } = TimeOfDay.Morning;
    public Season CurrentSeason { get; set; } = Season.Spring;

    [Signal] public delegate void TimeOfDayChangedEventHandler(int newTime);
    [Signal] public delegate void SeasonChangedEventHandler(int newSeason);

    public override void _Process(double delta)
    {
        _runningTime += (float)delta;
        _currentTime += (float)delta;

        if (_runningTime >= DayLength / Enum.GetValues(typeof(TimeOfDay)).Length)
        {
            ChangeTimeOfDay();
        }

        if (_currentTime >= DayLength)
        {
            _runningTime = 0;
            _currentTime = 0;
            CurrentDay++;
        }

        if (_currentTime == 0 && CurrentDay % 2 == 0)
        {
            ChangeSeason();
        }
    }

    public void ChangeTimeOfDay()
    {
        _runningTime = 0;
        CurrentTimeOfDay = (TimeOfDay)(((int)CurrentTimeOfDay + 1) % Enum.GetValues(typeof(TimeOfDay)).Length);
        GD.Print($"Time of day changed to: {CurrentTimeOfDay}");
        EmitSignal(SignalName.TimeOfDayChanged, (int)CurrentTimeOfDay);
    }

    private void ChangeSeason()
    {
        CurrentSeason = (Season)(((int)CurrentSeason + 1) % Enum.GetValues(typeof(Season)).Length);
        GD.Print($"Season changed to: {CurrentSeason}");
        EmitSignal(SignalName.SeasonChanged, (int)CurrentSeason);
    }
}
