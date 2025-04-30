using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public partial class GameManager : Node2D
{
	[Export] public PackedScene AnimalScene;
	[Export] public HUD _HUD;
	public Dictionary<Area2D, List<Animal>> fencedAreas = new();

	public override void _Ready()
	{
		foreach (Node node in GetTree().GetNodesInGroup("fencedAreas"))
		{
			if (node is Area2D area)
			{
				RegisterFencedArea(area);
			}
		}
		foreach (Node node in GetTree().GetNodesInGroup("animals"))
		{
			if (node is Animal animal)
			{
				_HUD.AddSheepCount(1);
				animal.AnimalDied += OnAnimalDied;
			}
		}
	}


	public override void _Process(double delta)
	{
		CheckGameCondition();
	}

	private void OnTimeOfDayChanged(int newTime)
	{
		TimeManager.TimeOfDay time = (TimeManager.TimeOfDay)newTime;
		switch (time)
		{
			case TimeManager.TimeOfDay.Morning:
				BreedAnimals();
				break;
			case TimeManager.TimeOfDay.Afternoon:
				break;
			case TimeManager.TimeOfDay.Evening:
				break;
			case TimeManager.TimeOfDay.Night:
				GD.Print("Go to bed!");
				break;
		}
	}

	private void RegisterFencedArea(Area2D area)
	{
		if (!fencedAreas.ContainsKey(area))
		{
			fencedAreas[area] = new List<Animal>();

			area.BodyEntered += (body) => OnAnimalEntered(area, body);
			area.BodyExited += (body) => OnAnimalExited(area, body);
		}
	}
	private void OnAnimalEntered(Area2D area, Node body)
	{
		if (body is Animal animal && !fencedAreas[area].Contains(animal))
		{
			fencedAreas[area].Add(animal);
			animal.CurrentFencedArea = area;
		}
	}

	private void OnAnimalExited(Area2D area, Node body)
	{
		if (body is Animal animal && fencedAreas[area].Contains(animal))
		{
			fencedAreas[area].Remove(animal);
			animal.CurrentFencedArea = null;
		}
	}
	private void OnAnimalDied(Animal animal)
	{
		if (animal.CurrentFencedArea != null)
			fencedAreas[animal.CurrentFencedArea].Remove(animal);
		_HUD.AddSheepCount(-1);
		animal.QueueFree();
	}

	private void BreedAnimals()
	{
		foreach (var area in fencedAreas.Keys)
		{
			BreedInFencedArea(area);
		}
	}

	private void BreedInFencedArea(Area2D area)
	{
		int currentCount = fencedAreas[area].Count;
		int spawnCount = currentCount / 2;

		GD.Print($"Breeding in fence: {area.Name}, spawning {spawnCount} new animals");

		for (int i = 0; i < spawnCount; i++)
		{
			SpawnAnimal(area);
		}
	}

	private void SpawnAnimal(Area2D area)
	{
		Animal newAnimal = (Animal)AnimalScene.Instantiate();
		GetTree().CurrentScene.AddChild(newAnimal);

		newAnimal.Position = area.Position + new Vector2(GD.Randf() * 50 - 25, GD.Randf() * 50 - 25);
		fencedAreas[area].Add(newAnimal);
		newAnimal.CurrentFencedArea = area;
		_HUD.AddSheepCount(1);
		newAnimal.AnimalDied += OnAnimalDied;
		Utils.Instance.player.inventory.AddItem("Milk", 1);
	}

	private void CheckGameCondition()
	{
		if (_HUD.sheepCount <= 0)
			_HUD.ShowGameOver();
		if (_HUD.sheepCount >= 10)
			_HUD.ShowGameWon();
	}

	private void OnRestartPressed()
	{
		GD.Print("Restarting...");
		GetTree().ReloadCurrentScene();
	}
}
