using Godot;
using System;

public class player : KinematicBody
{
	[Export]
	public int speed = 5;
	[Export]
	public int gravity = 80;

	public override void _Ready()
	{
		//var animPlayer = GetNode<AnimationPlayer>("Swat/RootNode/AnimationPlayer2");
		//var animTree = GetNode<AnimationTree>("Swat/RootNode/AnimationTree");
		//AnimationNodeStateMachinePlayback stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
		//stateMachine.Travel("Default");
	}

	public override void _PhysicsProcess(float delta)
	{
		var velocity = Vector3.Zero;
		if (Input.IsActionPressed("move_right"))
		{
			velocity.x = -speed;
		}
		if (Input.IsActionPressed("move_left"))
		{
			velocity.x = speed;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			velocity.z = speed;
		}
		if (Input.IsActionPressed("move_back"))
		{
			velocity.z = -speed;
		}
		velocity.y -= gravity * delta;
		MoveAndSlide(velocity, Vector3.Up);
	}
}
