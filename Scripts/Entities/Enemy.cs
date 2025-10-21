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
            if (Engine.IsEditorHint()) return;
            _trackedPlayer = value;
            _trackedPlayer.OnDeath += OnPlayerDeath;
        }
    }

    [Export] public bool Enabled = true;
    [Export] public float SightRange = 300.0f;
    [Export] public float Closeness = 200.0f;
    [Export] public float Accuracy;
    
    public override void _PhysicsProcess(double delta) {
        if (!Enabled) return;
        EnemyProcess(delta);
        base._PhysicsProcess(delta);
        
    }

    public void OnPlayerSpawn(Player player) {
        TrackedPlayer = player;
    }

    protected bool InPlayerRange() {
        return (_trackedPlayer?.GlobalPosition - GlobalPosition)?.Abs().Length() <= SightRange;
    }

    protected bool InCloseRange() {
        return (_trackedPlayer?.GlobalPosition - GlobalPosition)?.Abs().Length() <= Closeness;
    }

    protected Vector2 DirectionToTrackedPlayer() {
        var randomOffsetVector = new Vector2(0, (float)(Globals.Random.NextDouble() * 2.0 - 1.0) * Accuracy);
        var position = TrackedPlayer.GlobalPosition + randomOffsetVector;
        return GlobalPosition.DirectionTo(position).Normalized();
    }

    protected void MoveToPlayer() {
        if (TrackedPlayer == null) return;
        if (InPlayerRange() && !InCloseRange()) {
            var dir = Globals.GetGameDirection(this, TrackedPlayer);
            float xmv = dir.Item1 == Globals.GameDirection.L
                ? - Speed
                :   Speed;
            LastDirection = dir.Item1;
            Velocity = new Vector2(xmv, Velocity.Y);
        }
        else {
            Velocity = new Vector2(0, Velocity.Y);
        }
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

    protected abstract void EnemyProcess(double delta);

    private void OnPlayerDeath(Character character) {
        _trackedPlayer = null;
    }
}
