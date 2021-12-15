using System;
using Godot;

public class MenuController : Node
{
	Button startButton;
	Button settingsButton;
	Button quitButton;

	public override void _Ready()
	{
		startButton = GetNode<Button>("MainMenu/StartButton");
		settingsButton = GetNode<Button>("MainMenu/SettingsButton");
		quitButton = GetNode<Button>("MainMenu/QuitButton");

		startButton.Connect("pressed", this, "_StartButtonPressed");
		quitButton.Connect("pressed", this, "_QuitButtonPressed");
	}

	public void _StartButtonPressed()
	{
		LoadWorld();
	}

	public void _QuitButtonPressed()
	{
		QuitGame();
	}

	private void LoadWorld()
	{
		GetTree().ChangeScene("res://scenes/world.tscn");
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}
}
