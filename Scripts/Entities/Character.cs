using System.Collections;

namespace Ionfall.Scripts.Entities;

using Ionfall.Scripts.Interfaces;
using Ionfall.Scripts.Objects;

using Godot;
using static Godot.GD;

public partial class Character : CharacterBody2D, IHittable {
    //signals
    [Signal] public delegate void OnDeathEventHandler(Character character);
    
    // protected
    protected AnimatedSprite2D Sprite;
    protected AudioStreamPlayer2D ShootSound;
    
    protected Globals.GameDirection SpriteDirection = Globals.GameDirection.R;
    protected Globals.GameDirection LastDirection {
        get => _lastDirection;
        set {
            if (_lastDirection == value) return;
            _lastDirection = value;
            Sprite.FlipH = SpriteDirection != _lastDirection;
        }
    }
    
    // private
    private Globals.GameDirection _lastDirection = Globals.GameDirection.R;
    private int _flashedTimes = 0;
    private Timer _flashTimer;
    // fields
    public enum CharacterType {
        Neutral, Ally, Enemy
    }

    private int _health = 100;
    [Export] public int Health {
        get => _health;
        private set {
            _health = value;
            if (_health > 0) return;
            EmitSignalOnDeath(this);
            QueueFree();
        }
    }

    private CharacterType _type = CharacterType.Neutral;
    
    [Export] public int Score;
    [Export] public CharacterType Type { get; protected set; }
    [Export] public int BulletSpawnDistance = 50;
    [Export] public int Speed = 400;
    [Export] public int FlashTimes = 6;
    [Export] public Color DefaultModulate = Color.Color8(255, 255, 255);
    [Export] public Color FlashModulate = Color.Color8(255, 0, 0);

    public override void _Ready() {
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        ShootSound = GetNode<AudioStreamPlayer2D>("ShootSound");
        _flashTimer = GetNode<Timer>("FlashTimer");
        _flashTimer.Timeout += OnFlashTimerTimeout;
    }

    public override void _PhysicsProcess(double delta) {
        ApplyGravity(delta);
        MoveAndSlide();
    }
    
    public void OnHit(Bullet bullet) {
        Health -= bullet.Damage;
        if (_flashTimer.IsStopped()) _flashTimer.Start();
    }

    private void ApplyGravity(double delta) {
        Velocity = new Vector2(Velocity.X, Velocity.Y + (GetGravity().Y * (float)delta));
    }

    private void OnFlashTimerTimeout() {
        _flashedTimes += 1;
        if (_flashedTimes >= FlashTimes) {
            Sprite.Modulate = DefaultModulate;
            _flashedTimes = 0;
            _flashTimer.Stop();
        }
        else {
            Sprite.Modulate = (Sprite.Modulate == DefaultModulate) 
                ? FlashModulate 
                : DefaultModulate;
        }
    }
}