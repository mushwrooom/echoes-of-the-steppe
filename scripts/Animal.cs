using Godot;
using System;
using System.Collections.Generic;

public partial class Animal : CharacterBody2D
{
	[Export] public float Speed = 30.0f;
    [Export] public float DetectionRadius = 100.0f;
    [Export] public float CohesionStrength = 0.2f;
    
    private Vector2 _targetVelocity = Vector2.Zero;
	Player _player = null;
	List<Animal> collective = new List<Animal>();
	private void _on_area_2d_body_entered(PhysicsBody2D body)
	{
		if (body is Player player)
			_player = player;
		else if (body is Animal animal)
			collective.Add(animal);
	}
	private void _on_area_2d_body_exited(PhysicsBody2D body)
	{
		if (body is Player _)
			_player = null;
		else if (body is Animal animal)
			collective.Remove(animal);
	}
	public override void _Process(double delta)
    {
        Vector2 separation = Vector2.Zero;
        Vector2 cohesion = Vector2.Zero;
        int count = 0;

        // Get nearby animals for flocking behavior
        foreach (Node node in GetTree().GetNodesInGroup("animals"))
        {
            if (node is Animal other && other != this)
            {
                float distance = Position.DistanceTo(other.Position);
                if (distance < DetectionRadius)
                {
                    separation += (Position - other.Position).Normalized();
                    cohesion += other.Position;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            cohesion = ((cohesion / count) - Position).Normalized() * CohesionStrength;
        }

        // React to player if nearby
        if (_player != null)
        {
            Vector2 fleeDirection = (Position - _player.Position).Normalized();
            _targetVelocity = fleeDirection * Speed;
        }
        else
        {
            _targetVelocity = (separation + cohesion).Normalized() * Speed;
        }

        Velocity = Velocity.Lerp(_targetVelocity, 0.1f);
        MoveAndSlide();
    }
}
