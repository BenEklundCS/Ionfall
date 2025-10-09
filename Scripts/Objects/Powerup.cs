using Ionfall.Scripts.Entities;

namespace Ionfall.Scripts.Objects;

using Godot;
using static Godot.GD;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ionfall.Scripts.Interfaces;

public partial class Powerup : Node2D, ISpawnable {
    public enum PowerupType {
        Health,
    }
    
    private readonly Dictionary<PowerupType, string> _powerupSprites = new() {
        { PowerupType.Health, "res://Assets/Sprites/Powerups/extralife.png" },
    };

    private PowerupType _typeBacking = PowerupType.Health;
    private PowerupType Type {
        get => _typeBacking;
        set {
            _typeBacking = value;
            _sprite.Texture = Load<Texture2D>(_powerupSprites[Type]);
        }
    }
    
    private Area2D _area;
    private Sprite2D _sprite;

    public override void _Ready() {
        AddToGroup("Powerups");
        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;
        _sprite = GetNode<Sprite2D>("Sprite2D");
        Type = GetRandomPowerupType();
    }
    
    public Node2D Spawn() {
        return (Powerup)Load<PackedScene>("res://Scenes/Entities/powerup.tscn").Instantiate();
    }
    
    public void ApplyEffect(Player player) {
        switch (_typeBacking) {
            case PowerupType.Health:
                player.Health += 10;
                break;
        }
        QueueFree();
    }

    private static PowerupType GetRandomPowerupType() {
        var values = Enum.GetValues<PowerupType>();
        return values[Globals.Random.Next(0, values.Length)];
    }

    private void OnBodyEntered(Node2D node) {
        if (node is Player player) {
            ApplyEffect(player);
        }
    }
}

