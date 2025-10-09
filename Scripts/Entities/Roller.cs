namespace Ionfall.Scripts.Entities;

using Godot;
using System;
using Ionfall.Scripts.Entities;

public partial class Roller : Enemy {

    [Export] public int Damage = 10;
    
    private Area2D _area;
    private Timer _cooldownTimer;

    private Player _inRangePlayer;

    public override void _Ready() {
        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;
        _area.BodyExited += OnBodyExited;
        
        _cooldownTimer = GetNode<Timer>("CooldownTimer");
        _cooldownTimer.Timeout += OnCooldownTimerTimeout;
        
        base._Ready();
    }

    protected override void EnemyProcess(double delta) {
        MoveToPlayer();
    }

    private void StartHitting(Player player) {
        _inRangePlayer = player;
        Hit();
        _cooldownTimer.Start();
    }

    private void StopHitting() {
        _inRangePlayer = null;
        _cooldownTimer.Stop();
    }

    private void Hit() {
        _inRangePlayer?.Hit(Damage);
    }

    private void OnCooldownTimerTimeout() {
        Hit();
    }
    
    private void OnBodyEntered(Node2D body) {
        if (body is Player player) {
            StartHitting(player);
        }
    }
    
    private void OnBodyExited(Node2D body) {
        if (body is Player player)
            StopHitting();
    }
}
