using Godot;
using System;

public partial class CarTrip : Node2D
{
  private Global global;
  bool changeScene = false;

  public override void _Ready()
{
    global = GetNode<Global>("/root/Global");
}

  public override void _Process(double delta) 
  {
    carTripDone();
  }
  public void _on_finish_body_entered(Node2D body)
{
    if (body.HasMethod("Drive"))
    {
        changeScene = true;
    }
}

  public void carTripDone()
  {
    if (changeScene)
    {
      GetTree().ChangeSceneToFile($"res://Scenes/{global.selectedCountry}/{global.selectedCountry.ToLower()}_plaza.tscn");
    }
  }
}