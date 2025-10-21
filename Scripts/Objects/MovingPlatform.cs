using Ionfall.Scripts.Entities;

namespace Ionfall.Scripts.Objects;

using Godot;
using static Godot.GD;
using System;

public partial class MovingPlatform : StaticBody2D {
    private Area2D _area;
    private Vector2 _initialPosition;
    private Vector2 _targetPosition;
    private bool _movingToTarget = true;
    
    [Export] public Vector2 MoveOffset;
    [Export] public float MoveSpeed = 100f;
    [Export] public bool Continuous = true;

    public override void _Ready() {
        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;
        _initialPosition = GlobalPosition;
        _targetPosition = _initialPosition + MoveOffset;
    }

    public override void _Process(double delta) {
        var bodies = _area.GetOverlappingBodies();
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta) {
        Move(delta);
        Bounce();
    }

    private void Move(double delta) {
        var dest = _movingToTarget ? _targetPosition : _initialPosition;
        var dir = dest - GlobalPosition;
        var step = Mathf.Min(MoveSpeed * (float)delta, dir.Length());
        GlobalPosition += dir.Normalized() * step;
    }
    
    private void Bounce() {
        if (!Continuous) return;
        if (GlobalPosition == (_movingToTarget ? _targetPosition : _initialPosition))
            _movingToTarget = !_movingToTarget;
    }

    private void OnBodyEntered(Node2D body) {
        if (body is Character character) {
            character.Velocity = new Vector2(character.Velocity.X, MoveSpeed);
        }
    }
}