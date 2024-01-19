using Godot;
using System;
using System.Collections.Generic;

public class Enemy : Spatial
{
	[Export]
	public int speed = 1;
	[Export]
	public float rangeSquare = 16f;
	[Export]
	public float minDistance = 0.5f;
	[Export]
	public int health = 100;

	public bool isWaitingPlayer = true;
	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback animStateMachine;
	private Spatial player;

	public override void _Ready()
	{
		animTree = GetNode<AnimationTree>("AnimationTree");
		animStateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
	}

	public override void _PhysicsProcess(float delta)
	{
		var dis =  this.Translation.DistanceSquaredTo(player.Translation);
		if (dis < rangeSquare && dis > minDistance) {
			isWaitingPlayer = false;
			// Follow player
			animStateMachine.Travel("Run");
			var velocity = player.Translation - this.Translation;
			velocity = velocity.Normalized() * speed * delta;
			//MoveAndSlide(velocity, Vector3.Up);
			this.Translation += velocity;
		}
		LookAt(-player.Translation, Vector3.Up);
	}

	public void RegisterPlayer(Spatial player)
	{
		this.player = player;
	}

	private void Attack()
	{
		
	}

	public void GetDamage(int damage)
	{
		health -= damage;
	}
}
