using Godot;
using System;

public partial class Car : CharacterBody2D
{
	public const float ForwardSpeed = 150.0f; 
	public const float VerticalSpeed = 150.0f; 
	public const float JumpVelocity = -400.0f;
	
	private float currentSpeed = 0.0f;
	private float accelerationRate = 30.0f;
	private bool isStarting = true;
	private float startDelay = 1.0f;
	private float delayTimer = 0.0f;
	private bool isGameOver = false;
	private Label gameOverLabel;

	public void Drive() 
	{
	 //Pass	
	}
	public override void _Ready()
	{
		currentSpeed = 0.0f;
		isStarting = true;
		delayTimer = 0.0f;
		isGameOver = false;
		
		gameOverLabel = new Label();
		gameOverLabel.Text = "Ai pierdut. Apasă R pentru a reîncepe sau Q pt meniu";
		gameOverLabel.HorizontalAlignment = HorizontalAlignment.Center;
		gameOverLabel.VerticalAlignment = VerticalAlignment.Center;
		
		gameOverLabel.AddThemeColorOverride("font_color", Colors.White);
		gameOverLabel.AddThemeFontSizeOverride("font_size", 12);
		
		var viewport = GetViewport().GetVisibleRect().Size;
		gameOverLabel.Position = new Vector2(-200, -150);
		gameOverLabel.Size = new Vector2(100, 100);
		
		gameOverLabel.Visible = false;
		GetNode<Camera2D>("Camera2D").AddChild(gameOverLabel);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isGameOver)
		{
			if (Input.IsActionJustPressed("ui_r"))
			{
				GetTree().ReloadCurrentScene();
			}
			else if (Input.IsActionJustPressed("ui_q"))
			{
				GetTree().ChangeSceneToFile("res://Scenes/levels_menu.tscn");
			}
			return;
		}
		
		Vector2 velocity = Velocity;

		if (isStarting)
		{
			delayTimer += (float)delta;
			
			if (delayTimer < startDelay)
			{
				velocity.X = 0;
				velocity.Y = 0;
				Velocity = velocity;
				MoveAndSlide();
				return;
			}
			else if (currentSpeed < ForwardSpeed)
			{
				currentSpeed += accelerationRate * (float)delta;
				if (currentSpeed >= ForwardSpeed)
				{
					currentSpeed = ForwardSpeed;
					isStarting = false;
				}
			}
		}

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		velocity.X = currentSpeed;

		float verticalInput = Input.GetAxis("ui_up", "ui_down");
		velocity.Y = verticalInput * VerticalSpeed;

		if (verticalInput == 0)
		{
			velocity.Y = Mathf.MoveToward(velocity.Y, 0, VerticalSpeed * 0.1f);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		CheckForCollisions();
	}
	
	private void CheckForCollisions()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			Node collider = collision.GetCollider() as Node;
			
			if (collider != null && collider.Name.ToString().Contains("Stone"))
			{
				GameOver();
				break;
			}
		}
	}
	
	private void GameOver()
	{
		if (!isGameOver)
		{
			isGameOver = true;
			currentSpeed = 0;
			gameOverLabel.Visible = true;

			if (!InputMap.HasAction("ui_r"))
			{
				InputMap.AddAction("ui_r");
				InputEventKey eventKeyR = new InputEventKey();
				eventKeyR.Keycode = Key.R;
				InputMap.ActionAddEvent("ui_r", eventKeyR);
			}
			
			if (!InputMap.HasAction("ui_q"))
			{
				InputMap.AddAction("ui_q");
				InputEventKey eventKeyQ = new InputEventKey();
				eventKeyQ.Keycode = Key.Q;
				InputMap.ActionAddEvent("ui_q", eventKeyQ);
			}
		}
	}
}