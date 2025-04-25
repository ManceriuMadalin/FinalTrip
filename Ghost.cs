using Godot;
using System;

public partial class Ghost : CharacterBody2D
{
  private Global global;
    [Export]
    private float MoveSpeed = 50.0f;
    [Export]
    private float DetectionRadius = 50.0f;
    
    private Label interactionLabel;
    private Area2D detectionArea;
    private CollisionShape2D detectionShape;
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private bool playerInRange = false;
    
    // Movement variables
    private Vector2 targetPosition;
    private bool isMoving = true;
    
    // Called when the node enters the scene tree for the first time
    public override void _Ready()
    {
    global = GetNode<Global>("/root/Global");
        // Initialize the random number generator
        rng.Randomize();
        
        // Set up detection area
        detectionArea = new Area2D();
        detectionArea.Name = "DetectionArea";
        AddChild(detectionArea);
        
        // Create circular collision shape for detection
        detectionShape = new CollisionShape2D();
        var circleShape = new CircleShape2D();
        circleShape.Radius = DetectionRadius;
        detectionShape.Shape = circleShape;
        detectionArea.AddChild(detectionShape);
        
        // Connect signals
        detectionArea.BodyEntered += _on_ghost_body_entered;
        detectionArea.BodyExited += _on_ghost_body_exited;
        
        // Create interaction label
        interactionLabel = new Label();
        interactionLabel.Text = "Press E to catch";
        interactionLabel.Visible = false;
        interactionLabel.Position = new Vector2(-60, -70);
        interactionLabel.Rotation = 0; // Ensure no rotation
        interactionLabel.Scale = new Vector2(1, 1); 
        AddChild(interactionLabel);
        
        // Set initial target position
        SetNewTargetPosition();
    }
    
    // Called every frame
    public override void _Process(double delta)
    {
        // Handle player input if in range
        if (playerInRange && Input.IsActionJustPressed("interact")) // E key is usually mapped to ui_accept
        {
          global.number_of_ghosts++;
            // Make the ghost disappear completely
            Visible = false;
            SetProcess(false);
            SetPhysicsProcess(false);
            
            // Remove collision detection
            if (GetNode<CollisionShape2D>("CollisionShape2D") != null)
                GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
            
            detectionShape.Disabled = true;
            isMoving = false;
            playerInRange = false;
            interactionLabel.Visible = false;
            
            // QueueFree() would completely remove the ghost from the scene
            // If you want it to permanently disappear, uncomment the next line:
            // QueueFree();
            
            // Or if you want it to reappear after a delay:
            var timer = new Timer();
            timer.OneShot = true;
            timer.WaitTime = 5.0f; // 5 seconds
            timer.Timeout += OnRespawnTimer;
            AddChild(timer);
            timer.Start();
        }
        
        // Handle continuous movement
        if (isMoving)
        {
            MoveTowardsTarget(delta);
        }
    }
    
    private void _on_ghost_body_entered(Node2D body)
    {
        // Check if the entered body is a Player
        if (body.IsInGroup("Player") || body.Name == "Player")
        {
            playerInRange = true;
            interactionLabel.Visible = true;
        }
    }
    
    private void _on_ghost_body_exited(Node2D body)
    {
        // Check if the exited body is a Player
        if (body.IsInGroup("Player") || body.Name == "Player")
        {
            playerInRange = false;
            interactionLabel.Visible = false;
        }
    }
    
    private void MoveTowardsTarget(double delta)
    {
        Vector2 direction = targetPosition - GlobalPosition;
        float distance = direction.Length();
        
        // If we're close to the target, set a new target
        if (distance < 10)
        {
            SetNewTargetPosition();
            return;
        }
        
        // Normalize direction and apply movement
        direction = direction.Normalized();
        Vector2 velocity = direction * MoveSpeed;
        
        // Update the position
        Velocity = velocity;
        MoveAndSlide();
        
        // Make the ghost face the direction it's moving
        if (direction.X > 0)
        {
            Scale = new Vector2(1, 1); // Face right
        }
        else if (direction.X < 0)
        {
            Scale = new Vector2(-1, 1); // Face left
        }
    }
    
    private void SetNewTargetPosition()
    {
        // Generate random position within specified boundaries
        float randomX = rng.RandfRange(100, 1400);
        float randomY = rng.RandfRange(100, 1100);
        
        // Set new target position
        targetPosition = new Vector2(randomX, randomY);
    }
    
    private void OnRespawnTimer()
    {
        // Make the ghost reappear at a new position
        SetNewTargetPosition();
        GlobalPosition = targetPosition;
        
        // Set a new target to move towards
        SetNewTargetPosition();
        
        // Make the ghost visible again
        Visible = true;
        // Re-enable processing
        SetProcess(true);
        SetPhysicsProcess(true);
        // Re-enable collision
        if (GetNode<CollisionShape2D>("CollisionShape2D") != null)
            GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
        
        detectionShape.Disabled = false;
        isMoving = true;
    }
}