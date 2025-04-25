using Godot;
using System;

public partial class Earth : Node2D
{
    private AnimatedSprite2D animatedSprite;
    private Global global;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    global = GetNode<Global>("/root/Global");
    }

    public override void _Process(double delta)
    {
        animatedSprite.Frame = global.frame;
    }
}