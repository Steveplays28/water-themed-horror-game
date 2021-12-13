using Godot;
using Godot.Collections;

public class WaypointController : Node
{
	public static Dictionary<int, Node> waypoints;

	public override void _Ready()
	{
		Array<Node> waypointNodes = new Array<Node>();
		foreach (Node node in GetNode("/root").GetChildren())
		{
			if (node is Waypoint)
			{
				waypointNodes.Add(node);
			}
		}

		foreach (Node node in waypointNodes)
		{
			waypoints.Add((int)node.Get("waypointId"), node);
		}
	}
}