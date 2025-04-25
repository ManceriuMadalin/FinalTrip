using Godot;
using System;
using System.Collections.Generic;

public partial class Spain_Level : Node2D
{
private Texture2D pointTexture;
  private Node2D arrowBull;
  private float moveSpeed = 100.0f;
  private int direction = 1;
  private float leftBound = 301.0f;
  private float rightBound = 492.0f;
  private int health = 3;
  private int points = 0;
  private AnimatedSprite2D bullAnimation;
  private List<Node2D> heartSprites = new List<Node2D>();
  private List<Node2D> pointSprites = new List<Node2D>();
  private PackedScene pointScene;
  
  public override void _Ready()
  {
	arrowBull = GetNode<Node2D>("Arrow_bull");
	
	bullAnimation = GetNode<AnimatedSprite2D>("Bull/AnimatedSprite2D");
	
	if (bullAnimation != null)
	{
	  bullAnimation.Play("default");
	}
	
	heartSprites.Add(GetNode<Node2D>("Heart"));
	heartSprites.Add(GetNode<Node2D>("Heart2"));
	heartSprites.Add(GetNode<Node2D>("Heart3"));
	
	pointTexture = GD.Load<Texture2D>("res://Assets/point.png");
  }

  public override void _Process(double delta)
  {
		if (points == 3) {
        GetTree().ChangeSceneToFile("res://Scenes/levels_menu.tscn");
		}
	if (bullAnimation != null && !bullAnimation.IsPlaying())
	{
	  bullAnimation.Play("default");
	}
	
	if (arrowBull != null)
	{
	  Vector2 currentPosition = arrowBull.Position;

	  float newX = currentPosition.X + (moveSpeed + points * 50) * direction * (float)delta;

	  if (newX >= rightBound)
	  {
		direction = -1;
		newX = rightBound;
	  }
	  else if (newX <= leftBound)
	  {
		direction = 1;
		newX = leftBound;
	  }

	  arrowBull.Position = new Vector2(newX, currentPosition.Y);
	  
	  if (Input.IsActionJustPressed("jump")) 
	  {
		if ((currentPosition.X >= 301 && currentPosition.X <= 349) || 
			(currentPosition.X >= 453 && currentPosition.X <= 492))
		{
		  DecreaseHealth();
		}
		
		if (currentPosition.X >= 385 && currentPosition.X <= 417)
		{
		  IncreasePoints();
		}
	  }
	}
  }
  
  private void DecreaseHealth()
  {
	if (health > 0)
	{
	  health -= 1;
	  GD.Print("Health decreased! Current health: " + health);
	  
	  if (health < heartSprites.Count)
	  {
		heartSprites[health].Visible = false;
	  }
	}
  }
  
  private void IncreasePoints()
  {
	points += 1;
	GD.Print("Point gained! Current points: " + points);
	
	AddPointSprite();
  }
  
  private void AddPointSprite()
  {
	float xPos = 580 + (pointSprites.Count * 25); 
	
	if (xPos <= 630) 
	{
	  Sprite2D pointSprite = new Sprite2D();
	  pointSprite.Texture = pointTexture;
	  pointSprite.Position = new Vector2(xPos, 120);
	  pointSprite.Scale = new Vector2(1.5f, 1.5f);
	  AddChild(pointSprite);
	  pointSprites.Add(pointSprite);
	}
  }
}
