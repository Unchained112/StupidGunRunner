using Godot;
using System;
using System.Collections.Generic;

public class Game : Spatial
{
	[Export]
	private PackedScene[] streetBlocks = new PackedScene[3];
	[Export]
	private PackedScene endStreet;
	[Export]
	private PackedScene enemy;
	[Export]
	public float moveSpeed = 2f;
	[Export]
	public float streetDistance = 20f;

	private Player player;
	private Spatial enemySpawnPos;
	private List<Enemy> enemyList = new List<Enemy>();
	private Queue<Spatial> streetInScene = new Queue<Spatial>();
	private Random random = new Random();
	private int streetCnt = 0;

	public override void _Ready()
	{
		InitGame();
	}

	public override void _PhysicsProcess(float delta)
	{
		ProcessStreets(delta);
		ProcessEnemys(delta);
	}

	private void InitGame(){
		for (int i = 0; i < 4; i ++)
		{
			AddStreetBlock(i);
		}
		enemySpawnPos = GetNode<Spatial>("EnemySpawnPos");
		player = GetNode<Player>("Player");
		player.RegisterEnemyList(enemyList);
	}

	private void ProcessStreets(float delta)
	{
		foreach (Spatial street in streetInScene)
		{
			street.SetTranslation(new Vector3(0, 0, street.Translation.z - moveSpeed * delta));
		}
	}

	private void ProcessEnemys(float delta)
	{
		foreach (Enemy em in enemyList)
		{
			if (em.isWaitingPlayer)
			{
				em.SetTranslation(new Vector3(0, 0, em.Translation.z - moveSpeed * delta));
			}
			if (em.health < 0)
			{
				enemyList.Remove(em);
				em.QueueFree();
			}
		}
	}

	private void AddStreetBlock(int i)
	{
		int randIdx = random.Next(0, streetBlocks.Length);
		Street st = (Street)streetBlocks[randIdx].Instance();
		streetInScene.Enqueue(st);
		st.SetTranslation(new Vector3(0, 0, streetDistance * i));
		st.Connect("PlayWalkNextStree", this, "OnPlayWalkNextStree");
		AddChild(st);
	}

	private void DeletStreeBlock()
	{
		streetCnt++;
		streetInScene.Dequeue().QueueFree();
	}

	private void AddEnemy()
	{
		// Add enemy based on street block count
		Enemy em = (Enemy)enemy.Instance();
		em.RegisterPlayer(player);
		em.SetTranslation(enemySpawnPos.Translation);
		enemyList.Add(em);
		AddChild(em);
	}

	public void OnPlayWalkNextStree(object body)
	{
		AddStreetBlock(3);
		AddEnemy();
		if (streetCnt == 0) {
			return;
		}
		DeletStreeBlock();
	}
}
