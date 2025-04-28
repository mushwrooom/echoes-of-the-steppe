using Godot;
using System;

public partial class Chunk : Control
{
    // UI elements we'll reference from the scene
    [Export] public Label ChunkInfoLabel;
    [Export] public Label WeatherLabel;
    [Export] public VBoxContainer AnimalList;
    [Export] public Button MoveButton;
    [Export] public Button InvestigateButton;

    private ChunkData currentChunk;

    public override void _Ready()
    {
        Hide();
        MoveButton.Pressed += OnMoveButtonPressed;
        InvestigateButton.Pressed += OnInvestigateButtonPressed;
    }

    // Called from Main.cs when a tile is clicked
    public void ShowChunkInfo(ChunkData chunkData)
    {
        currentChunk = chunkData;

        ChunkInfoLabel.Text = $"Terrain: {chunkData.TerrainType}\n" +
                              $"Grass: {chunkData.GrassAmount}\n" +
                              $"Water: {(chunkData.HasWater ? "Yes" : "No")}\n" +
                              $"Wind: {chunkData.WindStrength:F2}";

        WeatherLabel.Text = $"Weather: {WeatherSystem.Instance.CurrentWeather}";

        PopulateAnimalList();

        Show();
    }

    private void PopulateAnimalList()
    {
        // Clear previous animal entries
        foreach (Node child in AnimalList.GetChildren())
        {
            child.QueueFree();
        }

        // Placeholder: Add animal info per chunk later if you track per-chunk animals
        foreach (var animal in GameState.Instance.PlayerAnimals)
        {
            var label = new Label
            {
                Text = $"{animal.Species} (Age {animal.Age})"
            };
            AnimalList.AddChild(label);
        }
    }

    private void OnMoveButtonPressed()
    {
        GD.Print("Move your animals and ger to this chunk.");
        // Add move logic here: update GameState.Instance.player_position or similar
    }

    private void OnInvestigateButtonPressed()
    {
        GD.Print("Investigating this area...");
        // Trigger random event or resource check
    }
}
