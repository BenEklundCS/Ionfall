namespace Ionfall.Scripts.Objects;

using Godot;
using System;

public partial class MovingPlatform : StaticBody2D {
    private Vector2 _initialPosition;
    private Vector2 _targetPosition;
    private bool _movingToTarget = true;
    
    [Export] public Vector2 MoveOffset;
    [Export] public float MoveSpeed;
    [Export] public bool Continuous;

    public override void _Ready() {
        _initialPosition = GlobalPosition;
        _targetPosition = _initialPosition + MoveOffset;
    }

    public override void _PhysicsProcess(double delta) {
        Move(delta);
        Bounce();
    }

    private void Move(double delta) {
        GlobalPosition += GlobalPosition
            .DirectionTo((_movingToTarget) 
                ? _targetPosition 
                : _initialPosition
            ) * (MoveSpeed * (float)delta);
        
    }

    private void Clamp() {
        GlobalPosition = (_targetPosition > _initialPosition)
            ? GlobalPosition.Clamp(_initialPosition, _targetPosition)
            : GlobalPosition.Clamp(_targetPosition, _initialPosition);
    }

    private void Bounce() {
        if (!Continuous) return;
        if ((GlobalPosition.DistanceTo(_targetPosition) < 1f && _movingToTarget) 
            || (GlobalPosition.DistanceTo(_initialPosition) < 1f && !_movingToTarget)
        ) {
            GlobalPosition = _movingToTarget 
                ? _targetPosition 
                : _initialPosition; 
            _movingToTarget = !_movingToTarget;
        }
    }
}
