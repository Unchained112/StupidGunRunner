using Godot;
using System;

public class Ak47 : Spatial
{
	private CPUParticles shootParticle;
	private AnimationPlayer animPlayer;

	public override void _Ready()
	{
		shootParticle = GetNode<CPUParticles>("CPUParticles");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void Shoot()
	{
		shootParticle.Emitting = true;
		animPlayer.Play("Shoot");
	}
}
