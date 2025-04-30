using Godot;
using System;

public partial class NPC : RigidBody2D
{
	[Signal]
	public delegate void StartDialogueEventHandler();
	private void _on_area_2d_body_entered(Node2D body)
    {
		if(body.Name == "Player")
        	EmitSignal(SignalName.StartDialogue);
    }
    private void _on_area_2d_body_exited(Node2D body)
    {
        
    }
}
