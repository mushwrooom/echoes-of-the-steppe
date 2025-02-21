using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 200.0f;
    [Export] public float HerdRadius = 120.0f;
    [Export] public float InfluenceStrength = 0.5f;
    AnimatedSprite2D animatedSprite2D;

    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    
    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        Velocity = inputDirection * Speed;
    }
    public int GetDirection()
    {
        if (Velocity == Vector2.Zero)
            return 0;

        return Velocity.X > 0 ? 1 : -1;
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }

    public override void _Process(double delta)
    {
        if(GetDirection() == 0)
        {
            animatedSprite2D.Play("idle");
        }
        else
        {
            animatedSprite2D.Play("running");
            animatedSprite2D.FlipH = GetDirection() != 1;
        }
    }
}
