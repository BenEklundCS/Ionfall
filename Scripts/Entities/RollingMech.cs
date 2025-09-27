namespace Ionfall.Scripts.Entities;

using Godot;
using System;

public partial class RollingMech : Enemy {
    private bool _shooting;
    protected override void EnemyProcess(double delta) {
        MoveToPlayer();
        Shoot();
    }

    protected override void HandleAnimation() {
        if (Velocity.Length() > 0) {
            if (!Sprite.IsPlaying()) {
                Sprite.Play();
            }
            if (_shooting) {
                Sprite.Animation = "moving_shooting";
            }
            else {
                Sprite.Animation = "moving";
            }
        }
        else {
            if (_shooting) {
                Sprite.Animation = "stationary_shooting";
            }
            else {
                Sprite.Animation = "moving";
                Sprite.Stop();
            }
        }
    }

    private void Shoot() {
        var inRange = InPlayerRange();
        _shooting = inRange;
        if (inRange) {
            Gun.Shoot(DirectionToTrackedPlayer());
        }
    }
}
