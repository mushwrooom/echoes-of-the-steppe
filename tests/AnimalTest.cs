using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class AnimalTest
{
    private Animal animal;

    [Before]
    public void Setup()
    {
        animal = new Animal();
    }

    [TestCase]
    public void test_initial_hunger_and_thirst()
    {
        AssertThat(animal.hunger).IsEqual(100);
        AssertThat(animal.thirst).IsEqual(100);
    }

    [TestCase]
    public void test_update_stats()
    {
        animal.UpdateStats(1.0f);
        AssertThat(animal.hunger).IsLess(100);
        AssertThat(animal.thirst).IsLess(100);
    }

    [TestCase]
    public void test_check_death()
    {
        animal.hunger = 0;
        animal.CheckDeath();
        AssertThat(animal.IsQueuedForDeletion()).IsTrue();
    }
}
