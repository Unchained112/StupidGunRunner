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
	public float moveSpeed = 5f;

	[Export]
	public float streetDistance = 20f;

	private Queue<Spatial> streetInScene = new Queue<Spatial>();

	private Random random = new Random();

	private int streetCnt = 0;

	public override void _Ready()
	{
		for (int i = 0; i < 4; i ++)
		{
			int randIdx = random.Next(0, streetBlocks.Length);
			Street st = (Street)streetBlocks[randIdx].Instance();
			streetInScene.Enqueue(st);
			st.SetTranslation(new Vector3(0, 0, streetDistance * i));
			st.Connect("PlayWalkNextStree", this, "OnPlayWalkNextStree");
			AddChild(st);
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		foreach (Spatial street in streetInScene)
		{
			street.SetTranslation(new Vector3(0, 0, street.Translation.z - moveSpeed * delta));
		}
	}

	public void OnPlayWalkNextStree(object body)
	{
		int randIdx = random.Next(0, streetBlocks.Length);
		Street st = (Street)streetBlocks[randIdx].Instance();
		streetInScene.Enqueue(st);
		st.SetTranslation(new Vector3(0, 0, streetDistance * 3));
		st.Connect("PlayWalkNextStree", this, "OnPlayWalkNextStree");
		AddChild(st);
		if (streetCnt == 0) {
			return;
		}
		streetCnt++;
		streetInScene.Dequeue().QueueFree();
	}
}
