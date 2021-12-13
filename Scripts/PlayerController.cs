using System;
using Godot;

public class PlayerController : RigidBody
{
	#region Variables - references
	private Camera camera;
	#endregion

	#region Variables - mouse
	public float sensX = 0.02f;
	public float sensY = 0.02f;
	public float maxRotationY = 90f;

	private float mouseMovementX;
	private float mouseMovementY;
	#endregion

	#region Variables - movement
	public Vector3 linearVelocityLocal;
	private float[] movementSpeed = new float[4];
	private float[] movementSpeedWalk = new float[] { 50000, 50000, 25000, 25000 };
	private float[] movementSpeedRun = new float[] { 100000, 100000, 50000, 50000 };
	private float[] movementSpeedCrouch = new float[] { 25000, 25000, 12500, 12500 };

	private Vector3 maxSpeed;
	private Vector3 maxSpeedWalk = new Vector3(1f, 1f, 1f);
	private Vector3 maxSpeedRun = new Vector3(2f, 2f, 2f);
	private Vector3 maxSpeedCrouch = new Vector3(0.5f, 0.5f, 0.5f);
	private float jumpForce = 250f;
	private bool isCrouching;
	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		Input.SetMouseMode(Input.MouseMode.Captured);

		camera = GetNode<Camera>("Camera");
		movementSpeed = movementSpeedWalk;
		maxSpeed = maxSpeedWalk;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		base._Process(delta);

		// Convert linear velocity from global to local
		linearVelocityLocal = Transform.basis.Orthonormalized().XformInv(LinearVelocity);

		// Mouse Y
		camera.RotationDegrees += new Vector3(-mouseMovementY, 0, 0);
		camera.RotationDegrees = new Vector3(Mathf.Clamp(camera.RotationDegrees.x, -maxRotationY, maxRotationY), camera.RotationDegrees.y, camera.RotationDegrees.z);

		mouseMovementY = 0;

		// Restarting and mouse escape
		if (Input.IsActionJustPressed("restart"))
		{
			var timeBeforeSceneChange = OS.GetTicksMsec();
			GetTree().ReloadCurrentScene();
			var timeAfterSceneChange = OS.GetTicksMsec();

			GD.Print("Scene reloaded in " + (timeAfterSceneChange - timeBeforeSceneChange).ToString() + "ms");
		}
		if (Input.IsActionJustPressed("escape"))
		{
			Input.SetMouseMode(Input.GetMouseMode() == Input.MouseMode.Visible ? Input.MouseMode.Hidden : Input.MouseMode.Visible);
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		// Movement
		if (Input.IsActionJustPressed("run"))
		{
			movementSpeed = movementSpeedRun;
			maxSpeed = maxSpeedRun;
		}
		else if (Input.IsActionJustReleased("run"))
		{
			movementSpeed = movementSpeedWalk;
			maxSpeed = maxSpeedWalk;
		}

		if (Input.IsActionJustPressed("crouch_hold"))
		{
			if (isCrouching)
			{
				isCrouching = false;
				movementSpeed = movementSpeedWalk;
				maxSpeed = maxSpeedWalk;
			}
			else
			{
				isCrouching = true;
				movementSpeed = movementSpeedCrouch;
				maxSpeed = maxSpeedCrouch;
			}
		}

		if (Input.IsActionJustPressed("crouch"))
		{
			movementSpeed = movementSpeedCrouch;
			maxSpeed = maxSpeedCrouch;
		}
		else if (Input.IsActionJustReleased("crouch"))
		{
			movementSpeed = movementSpeedWalk;
			maxSpeed = maxSpeedWalk;
		}

		if (Mathf.Abs(linearVelocityLocal.z) < maxSpeed.z)
		{
			if (Input.IsActionPressed("move_forward"))
			{
				AddForce(-Transform.basis.z * movementSpeed[0] * delta, Vector3.Zero);
			}
			if (Input.IsActionPressed("move_backwards"))
			{
				AddForce(Transform.basis.z * movementSpeed[1] * delta, Vector3.Zero);
			}
		}
		if (Mathf.Abs(linearVelocityLocal.x) < maxSpeed.x)
		{
			if (Input.IsActionPressed("move_right"))
			{
				AddForce(Transform.basis.x * movementSpeed[2] * delta, Vector3.Zero);
			}
			if (Input.IsActionPressed("move_left"))
			{
				AddForce(-Transform.basis.x * movementSpeed[3] * delta, Vector3.Zero);
			}
		}

		if (Input.IsActionJustPressed("jump"))
		{
			ApplyImpulse(Vector3.Zero, Transform.basis.y * jumpForce);
		}
	}

	public override void _IntegrateForces(PhysicsDirectBodyState state)
	{
		base._IntegrateForces(state);

		// Mouse X
		state.AngularVelocity = -mouseMovementX * sensX * Transform.basis.y * 100;
		mouseMovementX = 0;
	}

	public override void _Input(InputEvent inputEvent)
	{
		base._Input(inputEvent);

		// Mouse input
		if (inputEvent is InputEventMouseMotion)
		{
			var inputEventMouseMotion = inputEvent as InputEventMouseMotion;

			mouseMovementX = inputEventMouseMotion.Relative.x;
			mouseMovementY = inputEventMouseMotion.Relative.y;
		}
	}

	private void InfiniteMouse()
	{
		// Mouse X
		if (GetViewport().GetMousePosition().x >= GetViewport().Size.x)
		{
			GetViewport().WarpMouse(new Vector2(0, GetViewport().GetMousePosition().y));
		}
		else if (GetViewport().GetMousePosition().x <= 0)
		{
			GetViewport().WarpMouse(new Vector2(GetViewport().Size.x, GetViewport().GetMousePosition().y));
		}

		// Mouse Y
		if (GetViewport().GetMousePosition().y >= GetViewport().Size.y)
		{
			GetViewport().WarpMouse(new Vector2(GetViewport().GetMousePosition().x, 0));
		}
		else if (GetViewport().GetMousePosition().y <= 0)
		{
			GetViewport().WarpMouse(new Vector2(GetViewport().GetMousePosition().x, GetViewport().Size.y));
		}
	}
}
