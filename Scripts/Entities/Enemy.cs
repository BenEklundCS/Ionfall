using Ionfall.Scripts.Interfaces;
using Ionfall.Scripts.Objects;

namespace Ionfall.Scripts.Entities;

using Godot;
using System;

public abstract partial class Enemy : Character {
    private Player _trackedPlayer;
    public Player TrackedPlayer {
        get => _trackedPlayer;
        set {
            _trackedPlayer = value;
            _trackedPlayer.OnDeath += OnPlayerDeath;
        }
    }

    [Export] public float SightRange = 300.0f;
    [Export] public float Closeness = 200.0f;

    protected bool InPlayerRange() {
        return (_trackedPlayer?.GlobalPosition - GlobalPosition)?.Abs().Length() <= SightRange;
    }

    private bool InCloseRange() {
        return (_trackedPlayer?.GlobalPosition - GlobalPosition)?.Abs().Length() <= Closeness;
    }

    protected void MoveToPlayer() {
        if (TrackedPlayer == null) return;
        var mv = Vector2.Zero;
        if (InPlayerRange() && !InCloseRange()) {
            var dir = Globals.GetGameDirectionTo(this, TrackedPlayer);
            mv.X = dir.Item1 == Globals.GameDirection.L
                ? - Speed
                :   Speed;
            LastDirection = dir.Item1;
        }
        Velocity = mv;
    }
    
    protected override void HandleAnimation() {
        if (Velocity.Length() > 0) {
            Sprite.Play();
        }
        else {
            Sprite.Stop();
            Sprite.Frame = 0;
        }
    }

    private void OnPlayerDeath(Character character) {
        _trackedPlayer = null;
    }
}
