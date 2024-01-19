using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody
{
	[Export]
	public int speed = 5;
	[Export]
	public int gravity = 80;
	[Export]
	public int health = 10;
	[Export]
	public int damage = 60;
	[Export]
	public PackedScene Ak47;

	public int weaponCnt = 1;
	private Spatial testAk;
	private List<Enemy> enemyList;
	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback animStateMachine;
	private Vector3 aimPos = new Vector3(0, 0, 1);
	private List<Ak47> weaponList;

	public override void _Ready()
	{
		animTree = GetNode<AnimationTree>("AnimationTree");
		animStateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
		weaponList.Add(GetNode<Ak47>("GunPos/AK47"));
	}

	public override void _PhysicsProcess(float delta)
	{
		var velocity = Vector3.Zero;
		if (Input.IsActionPressed("move_right"))
		{
			velocity.x = -1;
		}
		if (Input.IsActionPressed("move_left"))
		{
			velocity.x = 1;
		}
		/*
		if (Input.IsActionPressed("move_forward"))
		{
			velocity.z = 1;
		}
		if (Input.IsActionPressed("move_back"))
		{
			velocity.z = -1;
		}
		*/
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

	public void RegisterEnemyList(List<Enemy> enemyList)
	{
		this.enemyList = enemyList;
	}

	private void Attack()
	{
		float minDist = 25f;
		Enemy curEnemy = enemyList[0];
		foreach (Enemy em in enemyList)
		{
			var temp = em.Translation.DistanceSquaredTo(this.Translation);
			if (temp < minDist)
			{
				minDist = temp;
				aimPos = em.Translation;
				curEnemy = em;
			}
		}
		curEnemy.GetDamage(damage * weaponCnt);
		for (Ak47 ak in weaponList)
		{
			ak.LookAt(aimPos, Vector3.Up);
		}
	}

	public void SetWeaponCnt(int cnt)
	{
		int cntDiff = cnt - weaponCnt;
		if (cntDiff > 0)
		{
			
		}
		else if (cntDiff < 0)
		{
			
		}
		weaponCnt = cnt;
	}

	public void UpdateWeaponList()
	{
		
	}
	
}
