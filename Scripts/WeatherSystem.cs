using Godot;

public partial class WeatherSystem : Node
{
    public static WeatherSystem Instance;

    public string CurrentWeather;
    public float WindDirection;

    public override void _Ready()
    {
        Instance = this;
        ChangeWeather();
    }

    public void ChangeWeather()
    {
        string[] possibleWeather = { "Clear", "Rain", "Snow", "Windy" };
        CurrentWeather = possibleWeather[GD.Randi() % possibleWeather.Length];
        WindDirection = (float)(GD.Randf() * 360.0f);
        GD.Print("Weather changed to: " + CurrentWeather);
    }
}
