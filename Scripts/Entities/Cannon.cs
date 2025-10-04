namespace Ionfall.Scripts.Entities;

using Godot;
using System;

[Tool]
public partial class Cannon : Enemy {
    private RectangleShape2D _detectionRect;
    private CollisionShape2D _shape;
    private Area2D _detection;
    private bool _init;
    private bool _playerDetected;
    private float _detectionRange;

    [Export]
    public float DetectionRange {
        get => _detectionRange;
        set {
            _detectionRange = value;
            SetSize();
        }
    }

    public override void _EnterTree() {
        Init();
    }

    public override void _Ready() {
        Init();
        base._Ready();
        Sprite.AnimationFinished += OnChargeAnimationFinished;
    }

    public override void _Process(double delta) {

    }

    public override void _PhysicsProcess(double delta) {

    }

    protected override void EnemyProcess(double delta) {
        
    }

    private void Init() {
        if (_init) return;

        _detection = GetNode<Area2D>("Detection");
        _detection.BodyEntered += OnBodyEntered;
        _detection.BodyExited += OnBodyExited;
        
        _shape = _detection.GetNode<CollisionShape2D>("CollisionShape2D");
        
        _detectionRect = (RectangleShape2D)_shape.Shape.Duplicate();
        _shape.Shape = _detectionRect;

        _init = true;
        
        SetSize();
    }

    private void SetSize() {
        if (!_init || _detectionRect == null || _shape == null) return;
        _detectionRect.Size = new Vector2(_detectionRect.Size.X, _detectionRange);
        _shape.Position = new Vector2(0, _detectionRange / 2f);
    }

    private void Shoot() {
        if (!Sprite.IsPlaying())
            Sprite.Play("charge");
    }

    private void OnChargeAnimationFinished() {
        if (Sprite.Animation != "charge") return;
        Gun.Shoot(Transform.Y);

        if (_playerDetected) {
            Shoot();
        }
        else {
            ResetAnimation();
        }
    }

    private void ResetAnimation() {
        Sprite.Animation = "idle";
        Sprite.Frame = 0;
    }

    private void OnBodyEntered(Node2D node) {
        if (node is not Player) return;
        Shoot();
        _playerDetected = true;
    }
    
    private void OnBodyExited(Node2D node) {
        if (node is Player) {
            _playerDetected = false;
        }
    }
}