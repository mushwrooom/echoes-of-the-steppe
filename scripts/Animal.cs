using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class Animal : CharacterBody2D
{
    [Export] public float Speed = 30.0f;
    [Export] public float DetectionRadius = 50.0f;
    [Export] public float CohesionStrength = 1.0f;

    private Vector2 _targetVelocity = Vector2.Zero;
    Player _player = null;
    List<Animal> collective = new List<Animal>();
    AnimatedSprite2D _animatedSprite2D;
    TimeManager _timeManager;
    String playingAnimation = "idle";

    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _timeManager = GetNode<TimeManager>("/root/Game/TimeManager");
    }
    public int GetVelocityDirection()
    {
        if (Velocity.Length() < 0.5f)
            return 0;

        return Velocity.X > 0 ? 1 : -1;
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

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (GetVelocityDirection() == 0)
        {
            if (_timeManager.CurrentTimeOfDay == TimeManager.TimeOfDay.Night)
            {
                _animatedSprite2D.Play("sleeping");
            }
            else
                _animatedSprite2D.Play("idle");
        }
        else
        {
            _animatedSprite2D.Play("running");
            _animatedSprite2D.FlipH = GetVelocityDirection() != 1;
        }
    }

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
}
