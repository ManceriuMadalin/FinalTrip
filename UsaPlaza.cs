using DialogueManagerRuntime;
using Godot;
using System;

public partial class UsaPlaza : Node2D
{
  private Global global;
  bool guards_text = false;
  bool car_text = false;
  bool dialogue_start = false;
  private CharacterBody2D cop;
  private AnimatedSprite2D copSprite;
  private CharacterBody2D cop2;
  private AnimatedSprite2D cop2Sprite;
  private CharacterBody2D cop3;
  private AnimatedSprite2D cop3Sprite;
  private CharacterBody2D cop4;
  private AnimatedSprite2D cop4Sprite;
  private CharacterBody2D cop5;
  private AnimatedSprite2D cop5Sprite;
  private float moveSpeed = 50.0f;
  private float[] minX = { 1290.0f, 1920.0f, 2060.0f };
  private float[] maxX = { 1590.0f, 2440.0f, 2450.0f };
  private bool[] movingRight = { true, true, true };
  private float[] minY = { 280.0f, 520.0f };
  private float[] maxY = { 600.0f, 900.0f };
  private bool[] movingDown = { true, true };

  public override void _Ready()
  {
    global = GetNode<Global>("/root/Global");

    cop = GetNode<CharacterBody2D>("%Cop");
    copSprite = cop.GetNode<AnimatedSprite2D>("Body");

    cop2 = GetNode<CharacterBody2D>("%Cop2");
    cop2Sprite = cop2.GetNode<AnimatedSprite2D>("Body");

    cop3 = GetNode<CharacterBody2D>("%Cop3");
    cop3Sprite = cop3.GetNode<AnimatedSprite2D>("Body");

    cop4 = GetNode<CharacterBody2D>("%Cop4");
    cop4Sprite = cop4.GetNode<AnimatedSprite2D>("Body");

    cop5 = GetNode<CharacterBody2D>("%Cop5");
    cop5Sprite = cop5.GetNode<AnimatedSprite2D>("Body");
  }

  public override void _Process(double delta)
  {
    movingRight[0] = MoveXCop(delta, cop, copSprite, movingRight[0], maxX[0], minX[0]);
    movingRight[1] = MoveXCop(delta, cop3, cop3Sprite, movingRight[1], maxX[1], minX[1]);
    movingRight[2] = MoveXCop(delta, cop4, cop4Sprite, movingRight[2], maxX[2], minX[2]);
    movingDown[0] = MoveYCop(delta, cop2, cop2Sprite, movingDown[0], maxY[0], minY[0]);
    movingDown[1] = MoveYCop(delta, cop5, cop5Sprite, movingDown[1], maxY[1], minY[1]);

    Guards_zone();
    Car_zone();
    Dialogues();
  }

  private bool MoveXCop(double delta, CharacterBody2D character, AnimatedSprite2D characterSprite, bool mr, float mu, float md)
  {
    float currentX = character.Position.X;
    characterSprite.Play("side_walk");

    if (mr)
    {
      currentX += moveSpeed * (float)delta;

      if (currentX >= mu)
      {
        currentX = mu;
        mr = false;
        character.Scale = new Vector2(-2.5f, 2.5f);
      }
    }
    else
    {
      currentX -= moveSpeed * (float)delta;

      if (currentX <= md)
      {
        currentX = md;
        mr = true;
        character.Scale = new Vector2(2.5f, 2.5f);
      }
    }

    character.Position = new Vector2(currentX, character.Position.Y);
    return mr;
  }

  private bool MoveYCop(double delta, CharacterBody2D character, AnimatedSprite2D characterSprite, bool mb, float mu, float md)
  {
    float currentY = character.Position.Y;

    if (mb)
    {
      currentY += moveSpeed * (float)delta;
      characterSprite.Play("walk_down");

      if (currentY >= mu)
      {
        currentY = mu;
        mb = false;
      }
    }
    else
    {
      currentY -= moveSpeed * (float)delta;
      characterSprite.Play("walk_up");

      if (currentY <= md)
      {
        currentY = md;
        mb = true;
      }
    }

    character.Position = new Vector2(character.Position.X, currentY);
    return mb;
  }

  public void _on_ufo_body_entered(Node2D body) {
    if (body.HasMethod("Player")){
        GetTree().ChangeSceneToFile("res://Scenes/levels_menu.tscn");
    }
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
    var secondLabel = GetNodeOrNull<Label>("%car_text2");
    if (label != null)
    {
      label.Text = car_text ? "Press E to enter the vehicle" : "";
      secondLabel.Text = car_text ? "Press E to enter the vehicle" : "";
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
}