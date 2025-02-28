using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq.Expressions;

public partial class Animal : CharacterBody2D
{
    [Export] public float Speed = 40.0f;
    [Export] public float DetectionRadius = 50.0f;
    [Export] public float CohesionStrength = 1.0f;

    [Signal] public delegate void AnimalDiedEventHandler(Animal animal);
    public Area2D CurrentFencedArea = null;

    float hunger = 100;
    float thirst = 100;


    Vector2 _targetVelocity = Vector2.Zero;
    Player _player = null;
    AnimatedSprite2D _animatedSprite2D;
    ProgressBar _hungerBar;
    ProgressBar _thirstBar;
    bool isOnPasture = false;
    bool nearWater = false;

    public override void _Ready()
    {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _hungerBar = GetNode<ProgressBar>("Stats/HungerBar");
        _thirstBar = GetNode<ProgressBar>("Stats/ThirstBar");
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
        UpdateStats((float)delta);
    }

    private void UpdateAnimation()
    {
        if (GetVelocityDirection() == 0)
        {
            if (Utils.Instance.IsNight())
            {
                _animatedSprite2D.Play("sleeping");
            }
            else if (isOnPasture || nearWater)
            {
                _animatedSprite2D.Play("grazing");
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

    private void UpdateStats(float delta)
    {
        float rechargeRate = 5;

        bool willGraze = !Utils.Instance.IsNight() &&
                         GetVelocityDirection() == 0 &&
                         isOnPasture && hunger < 100;


        hunger = willGraze ? Mathf.Min(hunger + rechargeRate * delta, 100) :
                             Mathf.Max(hunger - delta, 0);

        thirst = nearWater ? Mathf.Min(thirst + rechargeRate * delta, 100) :
                             Mathf.Max(thirst - delta, 0);


        _hungerBar.Visible = willGraze || hunger < 50;
        _hungerBar.Value = _hungerBar.MaxValue * (hunger / 100.0f);

        _thirstBar.Visible = nearWater || thirst < 50;
        _thirstBar.Value = _thirstBar.MaxValue * (thirst / 100.0f);

        CheckDeath();
    }

    private void CheckDeath()
    {
        if (thirst <= 0 || hunger <= 0)
        {
            EmitSignal(SignalName.AnimalDied, this);
        }
        else if (Utils.Instance.TimeManager.CurrentSeason == TimeManager.Season.Winter &&
                 CurrentFencedArea == null && Utils.Instance.IsNight())
        {
            EmitSignal(SignalName.AnimalDied, this);
        }
    }

    private void _on_area_2d_body_entered(Node2D body)
    {
        if (body is Player player)
            _player = player;
    }
    private void _on_area_2d_body_exited(Node2D body)
    {
        if (body is Player _)
            _player = null;
    }

    private void _on_interact_area_body_entered(Node2D body)
    {
        if (body.Name == "Pasture")
            isOnPasture = true;
        else
            nearWater = true;
    }
    private void _on_interact_area_body_exited(Node2D body)
    {
        if (body.Name == "Pasture")
            isOnPasture = false;
        else
            nearWater = false;
    }
}
