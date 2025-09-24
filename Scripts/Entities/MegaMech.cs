using Ionfall.Scripts.Objects;

namespace Ionfall.Scripts.Entities;

using Godot;
using System;

public partial class MegaMech : Enemy {
    private Timer _shootTimer;
    private readonly Bullet _bulletFactory = new ();
    
    public override void _Ready() {
        _shootTimer = GetNode<Timer>("ShootTimer");
        _shootTimer.Timeout += OnShootTimerTimeout;
        base._Ready();
    }
    
    protected override void EnemyProcess(double delta) {
        MoveToPlayer();
    }
    
    private void OnShootTimerTimeout() {
        if (InPlayerRange()) {
            Gun.Shoot(GlobalPosition.DirectionTo(TrackedPlayer.GlobalPosition).Normalized());
        }
    }
}
