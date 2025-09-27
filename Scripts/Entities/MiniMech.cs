using Ionfall.Scripts.Objects;

namespace Ionfall.Scripts.Entities;

using Godot;
using static Godot.GD;

using System;

public partial class MiniMech : Enemy {

    private Timer _shootTimer;
    
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
            Gun.Shoot(DirectionToTrackedPlayer());
        }
    }
}
