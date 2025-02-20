using Godot;
using System;
using System.Collections.Generic;

public partial class Animal : CharacterBody2D
{
    [Export] public float Speed = 30.0f;
    [Export] public float DetectionRadius = 50.0f;
    [Export] public float CohesionStrength = 1.0f;

    private Vector2 _targetVelocity = Vector2.Zero;
    Player _player = null;
    List<Animal> collective = new List<Animal>();
    private void _on_area_2d_body_entered(Node2D body)
    {
        if (body is Player player)
            _player = player;
        else if (body is Animal animal)
            collective.Add(animal);
    }
    private void _on_area_2d_body_exited(Node2D body)
    {
        if (body is Player _)
            _player = null;
        else if (body is Animal animal)
            collective.Remove(animal);
    }
    public override void _Process(double delta)
    {
        Vector2 center = Vector2.Zero;
        int count = 0;

        // Get nearby animals for flocking behavior
        foreach (Node node in GetTree().GetNodesInGroup("animals"))
        {
            if (node is Animal animal)
            {
                float distance = Position.DistanceTo(animal.Position);
                if (distance < DetectionRadius)
                {
                    center += animal.Position;
                    count++;
                }
            }
        }

        center /= count;

        // React to player if nearby
        if (_player != null)
        {
            Vector2 fleeDirection = (center - _player.Position).Normalized();
            _targetVelocity = fleeDirection * Speed;
        }
        else
        {
            _targetVelocity = Vector2.Zero;
        }

        Velocity = Velocity.Lerp(_targetVelocity, 0.1f);
        MoveAndSlide();
    }
}
