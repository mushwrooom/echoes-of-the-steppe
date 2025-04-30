using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;

public partial class Player : CharacterBody2D
{
    public float Speed { get; set; } = 200.0f;
    public float HerdRadius = 120.0f;
    public float InfluenceStrength = 0.5f;
    AnimatedSprite2D animatedSprite2D;
    private bool _isSleeping = false;
    private bool onWater = false;
    public Inventory inventory;

    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        inventory = GetNode<Inventory>("Inventory");
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
        if(Utils.Instance.WeatherSystem.CurrentWeather == WeatherSystem.WeatherType.Sunny)
            Speed = 200;
        else
            Speed = 140;
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

        if (onWater)
        {

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
		    Utils.Instance.player.inventory.AddItem("Meat", 1);
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
