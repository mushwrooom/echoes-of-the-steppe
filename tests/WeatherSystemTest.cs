using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class WeatherSystemTest
{
    private WeatherSystem weatherSystem;

    [Before]
    public void Setup()
    {
        weatherSystem = new WeatherSystem();
    }

    [TestCase]
    public void test_initial_weather()
    {
        AssertThat(weatherSystem.CurrentWeather).IsEqualTo(WeatherSystem.WeatherType.ExtremeCold);
    }

    [TestCase]
    public void test_apply_weather()
    {
        weatherSystem.CurrentWeather = WeatherSystem.WeatherType.Sunny;
        weatherSystem.ApplyWeather();
        AssertThat(weatherSystem.CurrentWeather).IsEqualTo(WeatherSystem.WeatherType.Sunny);
    }

    [TestCase]
    public void test_change_weather()
    {
        weatherSystem.OnSeasonChanged(TimeManager.Season.Spring);
        weatherSystem.ChangeWeather();
        AssertThat(weatherSystem.CurrentWeather).IsIn(WeatherSystem.WeatherType.Sunny, WeatherSystem.WeatherType.Snowy, WeatherSystem.WeatherType.Rainy, WeatherSystem.WeatherType.ExtremeCold);
    }
}
