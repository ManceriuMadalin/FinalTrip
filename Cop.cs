using Godot;
using System;
using System.Collections.Generic;

public partial class Cop : CharacterBody2D
{
    private Node2D _light;
    private AnimatedSprite2D _animatedSprite;

    private Dictionary<string, float> _lightRotations = new Dictionary<string, float>
    {
        { "side_walk", 0 },              
        { "walk_down", Mathf.Pi / 2 },    
        { "walk_up", -Mathf.Pi / 2 }     
    };

    public override void _Ready()
    {
        _light = GetNode<Node2D>("%Light");
        _animatedSprite = GetNode<AnimatedSprite2D>("%Body");
        _animatedSprite.AnimationChanged += _on_animation_changed;
        UpdateLightRotation();
    }

    public void _on_cop_body_entered(Node2D body) {
        if (body.HasMethod("Player")) {
				GetTree().ReloadCurrentScene();
        }
    }

    public void _on_cop_body_exited(Node2D body) {}

    private void _on_animation_changed()
    {
        UpdateLightRotation();
    }

    private void UpdateLightRotation()
    {
        string currentAnimation = _animatedSprite.Animation;

        if (_lightRotations.ContainsKey(currentAnimation))
        {
            _light.Rotation = _lightRotations[currentAnimation];
        }
    }
}
