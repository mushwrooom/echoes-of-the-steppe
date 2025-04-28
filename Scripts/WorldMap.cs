using Godot;
using System;
using System.Collections.Generic;

public partial class WorldMap : Control
{
    [Export] public TileMapLayer TileMap;

    public void GenerateWorld()
    {
        var random = new Random();
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                var data = new ChunkData
                {
                    TerrainType = "Plain",
                    GrassAmount = random.Next(10, 50),
                    HasWater = random.NextDouble() < 0.3,
                    WindStrength = (float)random.NextDouble()
                };

                GameState.Instance.WorldChunks[new Vector2I(x, y)] = data;
            }
        }
    }
}
