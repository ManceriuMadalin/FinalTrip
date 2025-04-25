using Godot;
using System;

public partial class ItalyLevel : Node2D
{
  private Global global;
  public override void _Ready()
  {
    global = GetNode<Global>("/root/Global");
  }
  public override void _Process(double delta)
  {
    if (global.number_of_rival == 2) {
        GetTree().ChangeSceneToFile("res://Scenes/levels_menu.tscn");
    }
  }
}
