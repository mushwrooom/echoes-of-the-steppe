using Godot;
using System;
using System.Collections.Generic;

public partial class Animal : RigidBody2D
{
	[Export]
	public int Speed = 50;
	Player playerRef = null;
	List<Animal> collective = new List<Animal>();
	private void _on_area_2d_body_entered(PhysicsBody2D body)
	{
		if (body is Player player)
			playerRef = player;
		else if (body is Animal animal)
			collective.Add(animal);
	}
	private void _on_area_2d_body_exited(PhysicsBody2D body)
	{
		if (body is Player _)
			playerRef = null;
		else if (body is Animal animal)
			collective.Remove(animal);
	}
	public override void _PhysicsProcess(double delta)
	{
		if (playerRef != null)
		{
			LookAt(playerRef.Position);
			Rotate(Mathf.DegToRad(180.0f));
			Vector2 avgDirection = Vector2.Zero;
			if (collective.Count > 0)
			{
				foreach (Animal animal in collective)
				{
					avgDirection += animal.LinearVelocity;
				}
				avgDirection = avgDirection.Normalized();
			}

			LinearVelocity = (Position - playerRef.Position + avgDirection*2).Normalized() * Speed;
			GD.Print(collective.Count);
		}
	}
}
