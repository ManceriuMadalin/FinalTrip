using Godot;
using System;

public partial class Menu : Control
{
	public override void _Ready()
	{
		GetNode<Button>("%PlayBtn").Pressed += _Play;
		GetNode<Button>("%QuitBtn").Pressed += _Quit;
	}

	public void _Play()
	{
		GetTree().ChangeSceneToFile(
			"res://Scenes/levels_menu.tscn"
		);
	}

	public void _Quit()
	{
		GetTree().Quit();
	}
}