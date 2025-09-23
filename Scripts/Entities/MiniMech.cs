using Ionfall.Scripts.Objects;

namespace Ionfall.Scripts.Entities;

using Godot;
using static Godot.GD;

using System;

public partial class MiniMech : Enemy {

    private Timer _shootTimer;
    private readonly Bullet _bulletFactory = new ();
    
    public override void _Ready() {
        Velocity = new Vector2(-Speed, 0);
        SpriteDirection = Globals.GameDirection.L;

        _shootTimer = GetNode<Timer>("ShootTimer");
        _shootTimer.Timeout += OnShootTimerTimeout;
        
        base._Ready();
    }

    public override void _PhysicsProcess(double delta) {
        HandleAi(TrackedPlayer);
        HandleAnimation();
        base._PhysicsProcess(delta);
    }

    private void HandleAi(Player player) {
        if (player == null) return;
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

    private void HandleAnimation() {
        if (Velocity.Length() > 0) {
            Sprite.Play();
        }
        else {
            Sprite.Stop();
            Sprite.Frame = 0;
        }
    }

    private void OnShootTimerTimeout() {
        if (InPlayerRange()) {
            Shoot();
        }
    }

    private void Shoot() {
        var bullet = (Bullet)_bulletFactory.Spawn();
        bullet.GlobalPosition = GlobalPosition;
        bullet.Direction = GlobalPosition.DirectionTo(TrackedPlayer.GlobalPosition).Normalized();
        bullet.Target = CharacterType.Ally;
        GetTree().CurrentScene.AddChild(bullet);
        ShootSound.Play();
    }
}
