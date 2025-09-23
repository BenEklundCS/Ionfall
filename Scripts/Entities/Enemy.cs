using Ionfall.Scripts.Interfaces;
using Ionfall.Scripts.Objects;

namespace Ionfall.Scripts.Entities;

using Godot;
using System;

public partial class Enemy : Character {
    private Player _trackedPlayer;
    public Player TrackedPlayer {
         get {
            if (_trackedPlayer != null && !IsInstanceValid(_trackedPlayer)) {
                _trackedPlayer = null;
            }
            return _trackedPlayer;
        }
        set => _trackedPlayer = value;
    }

    [Export] public float SightRange = 300.0f;
    [Export] public float Closeness = 200.0f;
    
    public override void _Ready() {
        Type = CharacterType.Enemy;
        base._Ready();
    }

    protected bool InPlayerRange() {
        return (_trackedPlayer?.GlobalPosition - GlobalPosition)?.Abs().Length() <= SightRange;
    }

    protected bool InCloseRange() {
        return (_trackedPlayer?.GlobalPosition - GlobalPosition)?.Abs().Length() <= Closeness;
    }
}
