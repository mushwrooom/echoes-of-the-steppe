using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 200.0f;
    [Export] public float HerdRadius = 120.0f;
    [Export] public float InfluenceStrength = 0.5f;

    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        Velocity = inputDirection * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
        
        // foreach (Node node in GetTree().GetNodesInGroup("animals"))
        // {
        //     if (node is Animal animal && Position.DistanceTo(animal.Position) < HerdRadius)
        //     {
        //         Vector2 pushDirection = (animal.Position - Position).Normalized();
        //         // animal.Velocity += pushDirection * InfluenceStrength * (Velocity.Length() / Speed);
        //         animal.Velocity += pushDirection * 10;
        //     }
        // }
    }
}
