using Godot;

public class VoicelineController : Node
{
	public bool isCurrentlyPlaying;

	private AudioStreamPlayer audioStreamPlayer;
	private RayCast rayCast;

	public override void _Ready()
	{
		base._Ready();

		audioStreamPlayer = GetNode<AudioStreamPlayer>("../AudioStreamPlayer");
		rayCast = GetNode<RayCast>("../Camera/RayCast");
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		if (rayCast.IsColliding())
		{
			Node collider = (Node)rayCast.GetCollider();

			if (collider.IsInGroup("Voiceline"))
			{
				PlayVoiceline(collider.GetNode("./VoicelineReference").Get("voicelinePath") as string, collider.GetNode("./VoicelineReference").Get("voicelineSubtitle") as string);
			}
		}
	}

	public void PlayVoiceline(string voicelinePath, string voicelineSubtitle)
	{
		if (!audioStreamPlayer.Playing)
		{
			audioStreamPlayer.Stream = GD.Load<AudioStreamSample>(voicelinePath);
			audioStreamPlayer.Play();

			GD.Print($"Playing voiceline: {voicelinePath} ----- Subtitle: \"{voicelineSubtitle}\"");
		}
		// DisplaySubtitle(voicelineSubtitle);
	}
}