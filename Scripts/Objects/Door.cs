namespace Ionfall.Scripts.Objects;

using Godot;
using System;
using Ionfall.Scripts.Interfaces;

public partial class Door : StaticBody2D, ITriggerable {
    private TileMapLayer _layer;
    private CollisionShape2D _shape;
    
    private bool _open;
    private bool _triggered;
    
    [Export] public bool Retriggerable;

    public override void _Ready() {
        _layer = GetNode<TileMapLayer>("TileMapLayer");
        _shape = GetNode<CollisionShape2D>("CollisionShape2D");
        ProcessMode = ProcessModeEnum.Always;
    }

    public void Trigger() {
        if (!Retriggerable && _triggered) return;
        
        _open = !_open;
        _layer.Visible = !_open;
        
        Callable.From(() => {
            _shape.Disabled = _open;
        }).CallDeferred();
        
        _triggered = true;
    }
}
