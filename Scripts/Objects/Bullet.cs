using System.Collections.Generic;
using Ionfall.Scripts.Entities;

namespace Ionfall.Scripts.Objects;

using Godot;
using static Godot.GD;
using System;
using Ionfall.Scripts.Interfaces;

public partial class Bullet : CharacterBody2D, ISpawnable {
    [Export] public float Speed = 1000.0f;
    [Export] public Vector2 Direction;
    [Export] public int Damage = 10;
    [Export] public float LifeTime = 0.5f;
    
    [Export]
    public Character.CharacterType Target = Character.CharacterType.Neutral;
    
    private Area2D _area;
    
    public override void _Ready() {
        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta) {
        Velocity = Direction * Speed;
        LifeTime -= (float)delta;
        if (LifeTime <= 0) {
            QueueFree();
        }
        MoveAndSlide();
    }

    public Node2D Spawn() {
        return (Node2D)Load<PackedScene>("res://Scenes/Objects/bullet.tscn").Instantiate();
    }

    private bool IsTarget(Character character) {
        return Target == Character.CharacterType.Neutral || character.Type == Target;
    }
    
    private void OnBodyEntered(Node2D body) {
        if (body is Bullet) return;
        if (body is Character character && IsTarget(character)) {
            character.Hit(Damage);
        };
        QueueFree();
    }
}
