using Godot;
using System;

public partial class TimeManager : Node
{
    public enum TimeOfDay { Morning, Afternoon, Evening, Night }
    public enum Season { Spring, Summer, Fall, Winter }

    [Export] public float DayLength = 60.0f;
    private float _currentTime = 0;
    private int _currentDay = 1;
    
    public TimeOfDay CurrentTimeOfDay { get; private set; } = TimeOfDay.Morning;
    public Season CurrentSeason { get; private set; } = Season.Spring;
    
    [Signal] public delegate void TimeOfDayChangedEventHandler(int newTime);
    [Signal] public delegate void SeasonChangedEventHandler(int newSeason);
    
    public override void _Process(double delta)
    {
        _currentTime += (float)delta;
        
        if (_currentTime >= DayLength)
        {
            _currentTime = 0;
            _currentDay++;
            ChangeTimeOfDay();
        }

        if (_currentDay % 30 == 0) // Every 30 days, change season
        {
            ChangeSeason();
        }
    }

    public void ChangeTimeOfDay()
    {
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
