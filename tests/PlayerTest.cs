using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class PlayerTest
{
    private Player player;

    [Before]
    public void Setup()
    {
        player = new Player();
    }

    [TestCase]
    public void test_initial_speed()
    {
        AssertThat(player.Speed).IsEqual(200.0f);
    }

    [TestCase]
    public void test_sleep_at_night()
    {
        Utils.Instance.TimeManager.CurrentTimeOfDay = TimeManager.TimeOfDay.Night;
        player.Sleep();
        AssertThat(Utils.Instance.TimeManager._currentTime).IsEqual(Utils.Instance.TimeManager.DayLength);
    }

    [TestCase]
    public void test_sleep_during_day()
    {
        Utils.Instance.TimeManager.CurrentTimeOfDay = TimeManager.TimeOfDay.Morning;
        player.Sleep();
        AssertThat(Utils.Instance.TimeManager._currentTime).IsNotEqual(Utils.Instance.TimeManager.DayLength);
    }
}
