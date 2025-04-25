using Godot;
using System;

public partial class Rival : Character
{
  private Global global;
    private float speed = 100.0f;
    private float chaseSpeed = 100.0f;
    private float attackDistance = 25.0f;
    
    private bool playerChase = false;
    private bool isAttacking = false;
    private string currentAnimation = "idle";
    
    private float attackCooldown = 3.0f;
    private float timeSinceLastAttack = 0.0f;
    
    private CharacterBody2D player = null;
    private AnimatedSprite2D animatedSprite;
    
    public override void _Ready()
    {
    global = GetNode<Global>("/root/Global");
        MaxHealth = 350;  
        CurrentHealth = MaxHealth;
        AttackDamage = 50; 
        
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        GD.Print($"Rival inițializat cu {MaxHealth} sănătate și {AttackDamage} damage de atac");
    }
    
    private float GetGravityValue()
    {
        return (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        
        if (!IsOnFloor())
        {
            velocity.Y += GetGravityValue() * (float)delta;
        }
        
        timeSinceLastAttack += (float)delta;
        
        if (player != null)
        {
            playerChase = true;
            
            if (!isAttacking)
            {
                Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
                
                velocity.X = direction.X * chaseSpeed;
                
                GD.Print("Moving toward player: Direction X = " + direction.X + ", Velocity X = " + velocity.X);
                
                if (direction.X != 0)
                {
                    animatedSprite.FlipH = direction.X < 0;
                }
                
                if (IsOnFloor())
                {
                    PlayAnimation("walk");
                }
                
                float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
                GD.Print("Distance to player: " + distanceToPlayer);
                
                if (distanceToPlayer <= attackDistance && timeSinceLastAttack >= attackCooldown)
                {
                    isAttacking = true;
                    timeSinceLastAttack = 0.0f;
                    PlayAnimation("attack");
                    animatedSprite.AnimationFinished += OnAttackAnimationFinished;
                    velocity.X = 0;
                    GD.Print("Attacking player!");
                    
                    if (player is Character)
                    {
                        Character playerCharacter = (Character)player;
                        playerCharacter.TakeDamage(AttackDamage);
                        GD.Print($"Rival a atacat player-ul pentru {AttackDamage} damage!");
                    }
                }
            }
        }
        else if (!isAttacking)
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, speed);
            if (IsOnFloor())
            {
                PlayAnimation("idle");
            }
        }
        
        Velocity = velocity;
        
        var collision = MoveAndSlide();
        
        GD.Print("Rival position: " + GlobalPosition);
    }
    
    private void PlayAnimation(string animName)
    {
        if (currentAnimation != animName)
        {
            currentAnimation = animName;
            animatedSprite.Play(animName);
            GD.Print("Playing animation: " + animName);
        }
    }
    
    private void OnAttackAnimationFinished()
    {
        isAttacking = false;
        animatedSprite.AnimationFinished -= OnAttackAnimationFinished;
        
        if (player != null && playerChase)
        {
            PlayAnimation("walk");
        }
        else
        {
            PlayAnimation("idle");
        }
    }
    
    public void _on_rival_body_entered(Node2D body)
    {
        GD.Print("Body entered: " + body.Name);
        if (body is CharacterBody2D)
        {
            player = (CharacterBody2D)body;
            playerChase = true;
            GD.Print("Player detected, chase activated");
        }
    }
    
    public void _on_rival_body_exited(Node2D body)
    {
        GD.Print("Body exited: " + body.Name);
        if (body is CharacterBody2D)
        {
            playerChase = false;
            player = null;
            GD.Print("Player lost, chase deactivated");
        }
    }
    
    public void SetPlayerTarget(CharacterBody2D playerNode)
    {
        player = playerNode;
        playerChase = true;
        GD.Print("Player manually set as target");
    }
    
    protected override void Die()
  {
    global.number_of_rival++;
    base.Die();
    PlayAnimation("idle");
    QueueFree();
  }
}