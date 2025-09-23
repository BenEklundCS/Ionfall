using System.Collections.Generic;
using Ionfall.Scripts.Entities;

namespace Ionfall.Scripts.Objects;

using Godot;
using static Godot.GD;
using System;
using Ionfall.Scripts.Interfaces;

public partial class Bullet : CharacterBody2D, ISpawnable {
    [Export] public SpriteFrames SpriteFrames;
    [Export] public float Speed = 1000.0f;
    [Export] public Vector2 Direction;
    [Export] public int Damage = 10;
    
    [Export]
    public Character.CharacterType Target = Character.CharacterType.Neutral;

    private AnimatedSprite2D _animatedSprite2D;
    private Area2D _area;
    
    public override void _Ready() {
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite2D.SpriteFrames = SpriteFrames;
        _animatedSprite2D.Play("default");
        _animatedSprite2D.AnimationFinished += OnAnimationFinished;
        
        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta) {
        Velocity = Direction * Speed;
        MoveAndSlide();
    }

    public Node2D Spawn() {
        return (Node2D)Load<PackedScene>("res://Scenes/Objects/bullet.tscn").Instantiate();
    }

    private void OnAnimationFinished() {
        QueueFree();
    }

    private void OnBodyEntered(Node2D body) {
        if (body is Character character && 
            (Target == Character.CharacterType.Neutral 
             || character.Type == Target)
            ) {
            character.OnHit(this);
            QueueFree();
        }
    }
}
