using Godot;
using System;

public partial class Main : Node
{
    [Export] public WorldMap WorldMap;
    [Export] public Chunk ChunkPreview;

    public override void _Ready()
    {
        GameState.Instance.LoadGameData();
        WorldMap.GenerateWorld();
    }

    public void OpenChunkPreview(ChunkData chunkData)
    {
        ChunkPreview.ShowChunkInfo(chunkData);
    }
}
