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
	[Export] public VBoxContainer InventoryBox;
	[Export] public VBoxContainer Prompt;
	[Export] public VBoxContainer Dialogue;
	[Export] public VBoxContainer Shop;
	[Export] public VBoxContainer Help;
	[Export] public Label MessageLabel;
	[Export] public Button RestartButton;
	[Export] public OptionButton CheatWeather;
	[Export] public OptionButton CheatTime;
	[Export] public OptionButton CheatSeason;
	public int sheepCount = 0;

	private TimeManager timeManager;
	private bool inDialogue = false;
	private Inventory _inventory;

	public override void _Ready()
	{
		base._Ready();
		_inventory = GetNode<Inventory>("/root/Game/Player/Inventory");
		UpdateInventoryDisplay();
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
	bool migrate = true;
	private void OnMigrateButtonPressed()
	{
		GetNode<Node2D>("/root/Game/TileMap").Position = new Vector2(migrate ? 1900 : 0, 0);
		GetNode<Node2D>("/root/Game/TileMap2").Position = new Vector2(!migrate ? 1900 : 0, 0);
		GetNode<Node2D>("/root/Game/TileMap").Visible = !migrate;
		GetNode<Node2D>("/root/Game/TileMap2").Visible = migrate;
		migrate = !migrate;
	}

	private void StartDialogue()
	{
		if (!Dialogue.Visible)
		{
			Dialogue.Visible = true;
		}
	}
	private void RequestHelp()
	{
		GetTree().CallGroup("animals", "GetHelp");
		Dialogue.Visible = false;
	}
	private void OfferHelp()
	{
		GetTree().CallGroup("animals", "OfferHelp");
		Utils.Instance.player.inventory.AddItem("Tugrik", 3);
		Dialogue.Visible = false;
	}
	private void StartShop()
	{
		if (!Shop.Visible)
		{
			Shop.Visible = true;
		}
	}
	private void BuyDeel()
	{
		if (_inventory.GetItemQuantity("Tugrik") >= 5)
		{
			_inventory.RemoveItem("Tugrik", 5);
			Utils.Instance.player.inventory.AddItem("Deel", 1);
			Shop.Visible = false;
		}
	}
	private void BuyAnimalClothing()
	{
		if (_inventory.GetItemQuantity("Tugrik") >= 5)
		{
			_inventory.RemoveItem("Tugrik", 5);
			GetTree().CallGroup("animals", "OnClothed");
			Shop.Visible = false;
		}
	}
	private void BuyStorage()
	{
		if (_inventory.GetItemQuantity("Tugrik") >= 5)
		{
			_inventory.RemoveItem("Tugrik", 5);
			Shop.Visible = false;
			_inventory._max += 10;
		}
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

	public void UpdateInventoryDisplay()
	{
		foreach (Node child in InventoryBox.GetChildren())
		{
			InventoryBox.RemoveChild(child);
			child.QueueFree();
		}

		foreach (var item in _inventory.GetItems())
		{
			var hbox = new HBoxContainer();
			var label = new Label();
			label.Text = $"{item.Key}: {item.Value}";
			hbox.AddChild(label);

			var sellButton = new Button();
			sellButton.Text = "Sell";
			sellButton.Connect("pressed", Callable.From(() => OnSellButtonPressed(item.Key)));
			hbox.AddChild(sellButton);

			InventoryBox.AddChild(hbox);
		}
	}

	private void OnSellButtonPressed(string itemName)
	{
		if (_inventory.GetItemQuantity(itemName) > 0)
		{
			_inventory.RemoveItem(itemName, 1);
			_inventory.AddItem("Tugrik", 1);
			UpdateInventoryDisplay();
		}
	}

	public void OnInventoryChanged()
	{
		UpdateInventoryDisplay();
	}
	public void OnShopButtonPressed()
	{
		if(!Shop.Visible)
		{
			StartShop();
		}
		else
		{
			Shop.Visible = false;
		}
	}
	public void OnHelpButtonPressed()
	{
		Help.Visible = !Help.Visible;
	}
}
