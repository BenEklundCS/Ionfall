using System.Collections;
using Ionfall.Scripts.Components;

namespace Ionfall.Scripts.Entities;

using Ionfall.Scripts.Interfaces;
using Ionfall.Scripts.Objects;

using Godot;
using static Godot.GD;

public abstract partial class Character : CharacterBody2D, IHittable {
    //signals
    [Signal] public delegate void OnDeathEventHandler(Character character);
    
    // protected
    protected AnimatedSprite2D Sprite;
    protected Gun Gun;
    protected Globals.GameDirection LastDirection {
        get => _lastDirection;
        set {
            if (_lastDirection == value) return;
            _lastDirection = value;
            Sprite.FlipH = SpriteDirection != _lastDirection;
        }
    }
    
    // private
    private Globals.GameDirection _lastDirection;
    private int _flashedTimes = 0;
    private Timer _flashTimer;
    // fields
    public enum CharacterType {
        Neutral, Ally, Enemy
    }

    private int _health = 100;
    [Export] public int Health {
        get => _health;
        set {
            _health = value;
            if (_health > 0) return;
            EmitSignalOnDeath(this);
            QueueFree();
        }
    }

    private CharacterType _type = CharacterType.Neutral;
    
    [Export] public int Score;
    [Export] public bool GravityEnabled = true;
    [Export] public Globals.GameDirection SpriteDirection;
    [Export] public CharacterType Type { get; protected set; }
    [Export] public int Speed = 400;
    [Export] public int FlashTimes = 6;
    [Export] public Color DefaultModulate = Color.Color8(255, 255, 255);
    [Export] public Color FlashModulate = Color.Color8(255, 0, 0);

    public override void _Ready() {
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        Sprite.Play();
        Gun = GetNode<Gun>("Gun");
        _flashTimer = GetNode<Timer>("FlashTimer");
        _flashTimer.Timeout += OnFlashTimerTimeout;
    }

    public override void _PhysicsProcess(double delta) {
        HandleAnimation();
        if (GravityEnabled)
            ApplyGravity(delta);
        MoveAndSlide();
    }
    
    public void Hit(int damage) {
        Health -= damage;
        if (_flashTimer.IsStopped()) _flashTimer.Start();
    }
    
    protected abstract void HandleAnimation();

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