using Godot;
using System;
using Ionfall.Scripts.Entities;
using Ionfall.Scripts.Interfaces;

public partial class Trigger : Area2D {
    [Export] public Node2D Triggerable;

    public override void _Ready() {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D node) {
        if (node is Player)
            ((ITriggerable)Triggerable).Trigger();
    }
}
