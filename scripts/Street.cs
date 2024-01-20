using Godot;
using System;

public class Street : Spatial
{
	[Signal]
	delegate void PlayWalkNextStreet(Node body);
}
