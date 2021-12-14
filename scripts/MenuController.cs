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

		quitButton.Connect("pressed", this, "_QuitButtonPressed");
	}

	public void _QuitButtonPressed()
	{
		QuitGame();
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}
}
