using DialogueManagerRuntime;
using Godot;
using System;

public partial class IrelandPlaza : Node2D
{
  private Global global;
  bool placard = false;
  bool elf = false;
  bool finish = false;
  bool dialogue_start = false;
  bool dialogue_elf = false;

  public override void _Ready()
  {
    global = GetNode<Global>("/root/Global");
  }

  public override void _Process(double delta)
  {
    Placard_zone();
    Dialogues();
    Dialogues_Elf();
    Finish();
  }

  public void _on_pot_body_entered(Node2D body)
  {
    if (body.HasMethod("Player"))
    {
      finish = true;
    }
  }

  public void _on_pot_body_exited(Node2D body)
  {
    if (body.HasMethod("Player"))
    {
      finish = false;
    }
  }

  public void _on_elf_boss_body_entered(Node2D body)
  {
    if (body.HasMethod("Player"))
    {
      elf = true;
    }
  }

  public void _on_elf_boss_body_exited(Node2D body)
  {
    if (body.HasMethod("Player"))
    {
      elf = false;
    }
  }

  public void _on_placard_body_entered(Node2D body)
  {
    if (body.HasMethod("Player"))
    {
      placard = true;
    }
  }

  public void _on_placard_body_exited(Node2D body)
  {
    if (body.HasMethod("Player"))
    {
      placard = false;
    }
  }

  public void Finish() {
    if (finish) {
        GetTree().ChangeSceneToFile("res://Scenes/levels_menu.tscn");
    }
  }

  public void Placard_zone()
  {
    var label = GetNodeOrNull<Label>("%placard");
    if (label != null)
    {
      label.Text = placard ? "Press E to read" : "";
      if (placard && Input.IsActionJustPressed("interact"))
      {
        dialogue_start = true;
        placard = false;
      }
    }
  }

  public void Dialogues()
  {
    if (dialogue_start)
    {
      dialogue_start = false;
      DialogueManager.ShowExampleDialogueBalloon(ResourceLoader.Load($"res://Dialogues/placard.dialogue"), "start");
    }
  }

  public void Dialogues_Elf()
  {
    if (elf)
    {
      elf = false;
      DialogueManager.ShowExampleDialogueBalloon(ResourceLoader.Load($"res://Dialogues/elf_boss.dialogue"), "start");
    }
  }
}
