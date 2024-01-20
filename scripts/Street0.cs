using Godot;
using System;

public class Street0 : Street
{
	[Export]
	public Area deleteArea;

	public override void _Ready()
	{
		deleteArea = GetNode<Area>("DeleteArea");
	}

	public void OnDeleteAreaBodyEnter(Node body)
	{
		if (body.IsInGroup("Player")){
			EmitSignal("PlayWalkNextStreet", body);
		}
	}

}
