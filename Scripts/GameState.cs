using Godot;
using System.Collections.Generic;

public partial class GameState : Node
{
    public static GameState Instance;

    public Dictionary<Vector2I, ChunkData> WorldChunks = new();
    public List<Animal> PlayerAnimals = new();

    public override void _Ready()
    {
        Instance = this;
    }

    public void LoadGameData()
    {
        // Placeholder: load JSON/world state
        GD.Print("Game data loaded.");
    }
}
