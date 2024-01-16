using Godot;
using System;

public class street0 : Spatial
{
	[Export]
	public Area deleteArea;

	[Signal]
	delegate void PlayWalkNextStree(Node body);

	public override void _Ready()
	{
		deleteArea = GetNode<Area>("DeleteArea");
		//GD.Print(deleteArea);
		//deleteArea.Connect("body_enter", this, "OnDeleteAreaBodyEnter");
		//GD.Print(deleteArea.GetSignalList());
	}

	public void OnDeleteAreaBodyEnter(Node body)
	{
		if (body.IsInGroup("Player")){
			EmitSignal("PlayWalkNextStree", body);
		}
	}

}
