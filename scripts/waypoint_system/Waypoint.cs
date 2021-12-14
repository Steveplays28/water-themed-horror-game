using System.Linq;
using Godot;

public class Waypoint : Area
{
	public int waypointId = 0;

	public override void _EnterTree()
	{
		int amountOfWaypoints = 0;
		foreach (Node node in GetNode("/root").GetChildren())
		{
			if (node is Waypoint)
			{
				amountOfWaypoints++;
			}
		}

		waypointId = amountOfWaypoints;
	}

	public override void _Ready()
	{
		base._Ready();

		Connect("gameplay_entered", this, "_GameplayEntered");
	}

	public void _GameplayEntered()
	{
		WaypointController.passedWaypoints.Add(WaypointController.passedWaypoints.Count, this);
		Visible = false;
	}
}
