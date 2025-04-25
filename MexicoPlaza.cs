using DialogueManagerRuntime;
using Godot;
using System;

public partial class MexicoPlaza : Node2D
{
  private Global global;
  bool guards_text = false;
  bool car_text = false;
  bool dialogue_start = false;

  public override void _Ready()
  {
    global = GetNode<Global>("/root/Global");
  }
  public override void _Process(double delta)
  {
    if (global.number_of_ghosts == 3) {
        GetTree().ChangeSceneToFile("res://Scenes/levels_menu.tscn");
    }
    Guards_zone();
    Car_zone();
    Dialogues();
  }

  public void _on_guards_zone_body_entered(Node2D body)
  {
    GD.Print("Body entered guards zone: " + body.Name);
    if (body.HasMethod("Player"))
    {
      guards_text = true;
    }
  }

  public void _on_guards_zone_body_exited(Node2D body)
  {
    if (body.HasMethod("Player"))
    {
      guards_text = false;
    }
  }

  public void _on_car_zone_body_entered(Node2D body)
  {
    GD.Print("Body entered guards zone: " + body.Name);
    if (body.HasMethod("Player"))
    {
      car_text = true;
    }
  }

  public void _on_car_zone_body_exited(Node2D body)
  {
    if (body.HasMethod("Player"))
    {
      car_text = false;
    }
  }

  public void Car_zone()
  {
    var label = GetNodeOrNull<Label>("%car_text");
    if (label != null)
    {
      label.Text = car_text ? "Press E to enter the vehicle" : "";
      if (car_text && Input.IsActionJustPressed("interact"))
      {
        GetTree().ChangeSceneToFile("res://Scenes/levels_menu.tscn");
      }
    }
  }

  public void Guards_zone()
  {
    var label = GetNodeOrNull<Label>("%guard_text");
    if (label != null)
    {
      label.Text = guards_text ? "Press E talk" : "";
      if (guards_text && Input.IsActionJustPressed("interact"))
      {
        dialogue_start = true;
        guards_text = false;
      }
    }
  }

  public void Dialogues()
  {
    if (dialogue_start)
    {
      dialogue_start = false;
      DialogueManager.ShowExampleDialogueBalloon(ResourceLoader.Load($"res://Dialogues/guard_conv_{global.selectedCountry.ToLower()}.dialogue"), "start");
    }
  }

  public void ChangeLimits()
  {
    switch (global.selectedCountry)
    //stanga sus dreapta jos
    {
      case "Italy":
        global.limit = [0, -400, 1040, 160];
        break;
      case "Spain":
        global.limit = [-208, 0, 845, 865];
        break;
      case "USA":
        global.limit = [0, 0, 3000, 1900];
        break;
      case "Australia":
        global.limit = [0, 950, 0, 850];
        break;
      case "Mexico":
        global.limit = [0, 900, 0, 800];
        break;
      case "Ireland":
        global.limit = [0, -560, 3800, 160];
        break;
      case "Egypt":
        global.limit = [0, 950, 0, 800];
        break;
      default:
        global.limit = [0, 0, 0, 0];
        break;
    }
  }
}
