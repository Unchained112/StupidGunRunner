using Godot;
using System;

public class Street : Spatial
{
	[Signal]
	delegate void PlayWalkNextStree(Node body);
}
