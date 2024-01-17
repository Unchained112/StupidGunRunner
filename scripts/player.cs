using Godot;
using System;

public class Player : KinematicBody
{
	[Export]
	public int speed = 5;
	[Export]
	public int gravity = 80;


	private Spatial testAk;

	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback animStateMachine;

	public override void _Ready()
	{
		animTree = GetNode<AnimationTree>("AnimationTree");
		animStateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
		testAk = GetNode<Spatial>("GunPos/AK47");
	}

	public override void _PhysicsProcess(float delta)
	{
		testAk.LookAt(new Vector3(0, 0, 1), new Vector3(0, 1, 0));
		var velocity = Vector3.Zero;
		if (Input.IsActionPressed("move_right"))
		{
			velocity.x = -1;
		}
		if (Input.IsActionPressed("move_left"))
		{
			velocity.x = 1;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			velocity.z = 1;
		}
		if (Input.IsActionPressed("move_back"))
		{
			velocity.z = -1;
		}
		velocity.y -= gravity * delta;
		if (velocity.x > 0)
		{
			animStateMachine.Travel("RunLeft");
		}
		else if (velocity.x < 0)
		{
			animStateMachine.Travel("RunRight");
		}
		else
		{
			animStateMachine.Travel("RunForward");
		}
		velocity.x *= speed;
		MoveAndSlide(velocity, Vector3.Up);
	}
}
