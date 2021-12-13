using Godot;

public class Waypoint : Spatial
{
	public int waypointId = 0;

	public override void _EnterTree()
	{
		waypointId = WaypointController.waypoints.Count;
	}
}