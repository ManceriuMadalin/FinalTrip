using Godot;
using System;

public partial class PlayerGravityBasedMovement : Character
{
    public const float Speed = 150.0f;
    public const float JumpVelocity = -300.0f;
    public AnimatedSprite2D animatedSprite;
    public Camera2D camera;
    private bool isAttacking = false;
    private bool jumpAnimationComplete = false;
    private string currentAnimation = "idle";
    private float attackRange = 30.0f; 

    public float GetGravityValue()
    {
        return (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
    }

    public override void _Ready()
    {
        MaxHealth = 500;
        CurrentHealth = MaxHealth;
        AttackDamage = 100;

        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        camera = GetNode<Camera2D>("Camera2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        bool isMoving = false;

        if (!IsOnFloor())
        {
            velocity.Y += GetGravityValue() * (float)delta;
            if (!jumpAnimationComplete && animatedSprite.Animation != "jump")
            {
                PlayAnimation("idle");
                animatedSprite.AnimationFinished += OnJumpAnimationFinished;
            }
        }
        else
        {
            if (jumpAnimationComplete)
            {
                jumpAnimationComplete = false;
                animatedSprite.AnimationFinished -= OnJumpAnimationFinished;
            }
            if (Input.IsActionJustPressed("jump"))
            {
                velocity.Y = JumpVelocity;
            }

            if (Input.IsActionJustPressed("attack") && !isAttacking)
            {
                isAttacking = true;
                PlayAnimation("attack");
                animatedSprite.AnimationFinished += OnAttackAnimationFinished;

                AttackNearbyEnemies();
            }
        }

        float directionX = 0;
        if (Input.IsActionPressed("move_left"))
            directionX -= 1.0f;
        if (Input.IsActionPressed("move_right"))
            directionX += 1.0f;

        if (directionX != 0)
        {
            isMoving = true;
            velocity.X = directionX * Speed;
            animatedSprite.FlipH = directionX < 0;
        }
        else
        {
            isMoving = false;
            velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
        }

        if (!isAttacking && IsOnFloor())
        {
            if (isMoving)
            {
                PlayAnimation("walk");
            }
            else
            {
                PlayAnimation("idle");
            }
        }

        Velocity = velocity;
        MoveAndSlide();

        UpdateCameraLimits();
    }

    private void AttackNearbyEnemies()
    {
        var spaceState = GetWorld2D().DirectSpaceState;

        var queryParameters = new PhysicsShapeQueryParameters2D
        {
            CollideWithAreas = true,
            CollideWithBodies = true,
            CollisionMask = 1, // Ajustează în funcție de layer-ele de coliziune
            Transform = new Transform2D(0, GlobalPosition),
            Shape = new CircleShape2D { Radius = attackRange }
        };

        var result = spaceState.IntersectShape(queryParameters);

        foreach (var collisionInfo in result)
        {
            var collider = collisionInfo["collider"].As<Node>();
            if (collider is Character && collider != this)
            {
                Character enemy = (Character)collider;
                enemy.TakeDamage(AttackDamage);
            }
        }
    }

    public void Player()
    {
        //pass
    }

    public void UpdateCameraLimits()
    {
        Global global = GetNode<Global>("/root/Global");

        if (!string.IsNullOrEmpty(global.selectedCountry))
        {
            global.UpdateLimitsGravity();

            camera.LimitLeft = (int)global.limit[0];
            camera.LimitTop = (int)global.limit[1];
            camera.LimitRight = (int)global.limit[2];
            camera.LimitBottom = (int)global.limit[3];
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

    private void OnJumpAnimationFinished()
    {
        jumpAnimationComplete = true;
        animatedSprite.Pause();
        animatedSprite.Frame = animatedSprite.SpriteFrames.GetFrameCount(animatedSprite.Animation) - 1;
    }

    private void OnAttackAnimationFinished()
    {
        isAttacking = false;
        animatedSprite.AnimationFinished -= OnAttackAnimationFinished;
        if (!IsOnFloor())
        {
            if (!jumpAnimationComplete)
            {
                PlayAnimation("idle");
                animatedSprite.AnimationFinished += OnJumpAnimationFinished;
            }
        }
        else if (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right"))
        {
            PlayAnimation("walk");
        }
        else
        {
            PlayAnimation("idle");
        }
    }

    protected override void Die()
    {
        base.Die();
        PlayAnimation("idle");
        QueueFree();
    }
}