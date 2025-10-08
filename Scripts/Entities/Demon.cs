namespace Ionfall.Scripts.Entities;

using Godot;
using System;
using Ionfall.Scripts.Entities;

public partial class Demon : Enemy {
    
    private Timer _shootTimer;
    
    public override void _Ready() {
        _shootTimer = GetNode<Timer>("ShootTimer");
        _shootTimer.Timeout += OnShootTimerTimeout;
        _shootTimer.Paused = !Enabled;
        base._Ready();
    }

    protected override void EnemyProcess(double delta) {
        MoveToPlayer();
    }
    
    private void OnShootTimerTimeout() {
        if (InPlayerRange()) {
            Gun.Shoot(DirectionToTrackedPlayer());
        }
    }
}
