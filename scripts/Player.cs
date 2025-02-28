using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 200.0f;
    [Export] public float HerdRadius = 120.0f;
    [Export] public float InfluenceStrength = 0.5f;
    AnimatedSprite2D animatedSprite2D;
    private bool _isSleeping = false;
    private bool onWater = false;

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
        if (GetDirection() == 0)
        {
            animatedSprite2D.Play("idle");
        }
        else
        {
            animatedSprite2D.Play("running");
            animatedSprite2D.FlipH = GetDirection() != 1;
        }

        if(onWater)  {

        }
    }

    public void Sleep()
    {
        if (!_isSleeping && Utils.Instance.IsNight())
        {
            GD.Print("Sleeping...");
            _isSleeping = true;
            Utils.Instance.TimeManager._currentTime = Utils.Instance.TimeManager.DayLength;
            Utils.Instance.TimeManager.ChangeTimeOfDay();
            _isSleeping = false;
        }
        else
        {
            GD.Print("You can only sleep at night!");
        }
    }

    private void _on_area_2d_body_entered(Node2D body)
    {
        if (!_isSleeping && body.Name == "Ger")
        {
            Sleep();
        }
        else if (body is TileMapLayer)
        {
            onWater = true;
        }
    }
    private void _on_area_2d_body_exited(Node2D body)
    {
        if (body is TileMapLayer)
        {
            onWater = false;
        }
    }
}
