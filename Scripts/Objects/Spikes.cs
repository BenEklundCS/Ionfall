using System;
using Godot;
using Ionfall.Scripts.Entities;
using Ionfall.Scripts.Interfaces;

[Tool]
public partial class Spikes : Node2D {
    private const int Base = 15;

    private Sprite2D _sprite;
    private Area2D _area;
    private RectangleShape2D _rectangle;
    private Timer _timer;
    private bool _init;

    private int _spikeCount;

    [Export]
    public int SpikeCount {
        get => _spikeCount;
        set {
            _spikeCount = value;
            SetSize();
        }
    }

    [Export] public int Damage = 10;

    public override void _EnterTree() {
        Init();
    }

    public override void _Ready() {
        Init();
    }

    private void Init() {
        if (_init) return;

        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;

        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;

        var shapeNode = _area.GetNode<CollisionShape2D>("CollisionShape2D");
        _rectangle = (RectangleShape2D)shapeNode.Shape.Duplicate();
        shapeNode.Shape = _rectangle;

        _sprite = GetNode<Sprite2D>("Sprite2D");
        _sprite.RegionEnabled = true;
        _sprite.TextureRepeat = CanvasItem.TextureRepeatEnum.Enabled;

        _init = true;

        SetSize();
    }

    private void SetSize() {
        if (!_init) return;

        var size = new Vector2(SpikeCount * Base, Base);
        _rectangle.Size = size;
        _sprite.RegionRect = new Rect2(_sprite.RegionRect.Position, size);
    }

    private void QueryForPlayer() {
        if (!_area.HasOverlappingBodies()) return;
        foreach (var body in _area.GetOverlappingBodies()) {
            if (body is Player player) {
                player.Hit(Damage);
            }
        }
    }

    private void OnBodyEntered(Node2D node) {
        if (node is IHittable hittable) {
            hittable.Hit(Damage);
            _timer.Start();
        }
    }

    private void OnTimerTimeout() {
        QueryForPlayer();
    }
}
