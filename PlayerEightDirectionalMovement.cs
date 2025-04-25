using Godot;
using System;

public partial class PlayerEightDirectionalMovement : CharacterBody2D
{
    public const float Speed = 150.0f;
    public AnimatedSprite2D animatedSprite;
    public Camera2D camera;
    private string currentAnimation = "idle_front";

    public void Player()
    {
        //Pass
    }
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        camera = GetNode<Camera2D>("Camera2D");
    }

    public override void _Process(double delta)
    {

        Global global = GetNode<Global>("/root/Global");
        Vector2 movement = Vector2.Zero;
        bool isMoving = false;

        if (Input.IsActionPressed("move_right"))
        {
            movement.X += Speed * (float)delta;
            PlayAnimation("walk_side");
            animatedSprite.FlipH = false;
            isMoving = true;
        }
        else if (Input.IsActionPressed("move_left"))
        {
            movement.X -= Speed * (float)delta;
            PlayAnimation("walk_side");
            animatedSprite.FlipH = true;
            isMoving = true;
        }
        else if (Input.IsActionPressed("move_down"))
        {
            movement.Y += Speed * (float)delta;
            PlayAnimation("walk_front");
            isMoving = true;
        }
        else if (Input.IsActionPressed("move_up"))
        {
            movement.Y -= Speed * (float)delta;
            PlayAnimation("walk_back");
            isMoving = true;
        }

        Position += movement;

        if (!isMoving)
        {
            if (currentAnimation == "walk_side")
                PlayAnimation("idle_side");
            else if (currentAnimation == "walk_front")
                PlayAnimation("idle_front");
            else if (currentAnimation == "walk_back")
                PlayAnimation("idle_back");
            else
                PlayAnimation("idle_front");
        }

        UpdateCameraLimits();
    }

    public void UpdateCameraLimits()
    {
        Global global = GetNode<Global>("/root/Global");

        if (!string.IsNullOrEmpty(global.selectedCountry))
        {
            global.UpdateLimits();

            camera.LimitLeft = (int)global.limit[0];
            camera.LimitTop = (int)global.limit[1];
            camera.LimitRight = (int)global.limit[2];
            camera.LimitBottom = (int)global.limit[3];

            GD.Print($"Camera limits set for {global.selectedCountry}: Left={camera.LimitLeft}, Right={camera.LimitRight}, Top={camera.LimitTop}, Bottom={camera.LimitBottom}");
        }
        else
        {
            GD.Print("Cannot update camera limits: No country selected");
        }
    }

    private void PlayAnimation(string animName)
    {
        if (currentAnimation != animName)
        {
            currentAnimation = animName;
            animatedSprite.Play(animName);
        }
    }
}