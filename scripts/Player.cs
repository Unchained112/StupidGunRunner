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
	public float range = 25f;
	[Export]
	public PackedScene Ak47Scene;

	public int weaponCnt = 0;
	private List<Enemy> enemyList;
	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback animStateMachine;
	private Vector3 aimPos = new Vector3(0, 0, 1);
	private List<Ak47> weaponList = new List<Ak47>();
	private Spatial gunPos;
	private Timer attackTimer;

	public override void _Ready()
	{
		animTree = GetNode<AnimationTree>("AnimationTree");
		animStateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
		gunPos = GetNode<Spatial>("GunPos");
		attackTimer = GetNode<Timer>("AttackTimer");
		AddWeapon();
		AddWeapon();
		AddWeapon();
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
		Attack();
	}

	public void RegisterEnemyList(List<Enemy> enemyList)
	{
		this.enemyList = enemyList;
	}

	private void Attack()
	{
		// Check attack timer
		if (attackTimer.IsStopped() && enemyList.Count > 0)
		{
			attackTimer.Start();
			// Check through all enemies
			float minDist = range - 1;
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
			// Enemy within attack range or not
			if (range < minDist)
			{
				return;
			}
			curEnemy.GetDamage(damage * weaponCnt);
			foreach (Ak47 ak in weaponList)
			{
				ak.LookAt(aimPos, Vector3.Up);
				ak.Shoot();
			}

		}
	}

	public void SetWeaponCnt(int cnt)
	{
		int cntDiff = cnt - weaponCnt;
		if (cntDiff > 0)
		{
			for (int i = 0; i < cntDiff; i++)
			{
				AddWeapon();
			}
		}
		else if (cntDiff < 0)
		{
			for (int i = 0; i < -cntDiff; i++)
			{
				RemoveWeapon();
			}
		}
		weaponCnt = cnt;
	}

	private void AddWeapon()
	{
		weaponCnt++;
		Ak47 ak = (Ak47)Ak47Scene.Instance();
		weaponList.Add(ak);
		AddChild(ak);
		UpdateWeaponPos();
	}

	private void RemoveWeapon()
	{
		if (weaponCnt == 1)
		{
			health -= 1;
			return;
		}
		else
		{
			weaponList.RemoveAt(weaponList.Count - 1);
			UpdateWeaponPos();
		}
	}

	private void UpdateWeaponPos()
	{
		for (int i = 0; i < weaponCnt; i++)
		{
			int lineLen = weaponCnt % 10;
			float x = (i - lineLen / 2) * 0.1f;
			float y = weaponCnt / 10 * 0.2f;
			weaponList[i].Translation = new Vector3(x, y, 0) + gunPos.Translation;
		}
	}
}
