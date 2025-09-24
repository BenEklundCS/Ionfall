using Ionfall.Scripts.Objects;

namespace Ionfall.Scripts.Entities;

using Godot;
using static Godot.GD;

using System;

public partial class MiniMech : Enemy {

    private Timer _shootTimer;
    private readonly Bullet _bulletFactory = new ();
    
    public override void _Ready() {
        _shootTimer = GetNode<Timer>("ShootTimer");
        _shootTimer.Timeout += OnShootTimerTimeout;
        base._Ready();
    }

    public override void _PhysicsProcess(double delta) {
        MoveToPlayer();
        base._PhysicsProcess(delta);
    }

    private void OnShootTimerTimeout() {
        if (InPlayerRange()) {
            Gun.Shoot(GlobalPosition.DirectionTo(TrackedPlayer.GlobalPosition).Normalized());
        }
    }
}
