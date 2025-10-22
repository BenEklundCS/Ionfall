using Ionfall.Scripts.Entities;

namespace Ionfall.Scripts.Objects;

using Godot;
using static Godot.GD;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Ionfall.Scripts.Interfaces;

public partial class Powerup : CharacterBody2D, ISpawnable {
    public enum PowerupType {
        Health,
        Ammo,
    }
    
    private readonly Dictionary<PowerupType, string> _powerupSprites = new() {
        { PowerupType.Health, "res://Assets/Sprites/Powerups/extralife.png" },
        { PowerupType.Ammo, "res://Assets/Sprites/Powerups/tripleshot.png"}
    };
    
    private PowerupType _typeBacking = PowerupType.Health;
    private PowerupType Type {
        get => _typeBacking;
        set {
            _typeBacking = value;
            _sprite.Texture = Load<Texture2D>(_powerupSprites[Type]);
        }
    }

    private AudioStreamPlayer2D _applyEffectSound;
    private Area2D _area;
    private Sprite2D _sprite;
    private CollisionShape2D _collisionShape;

    public override void _Ready() {
        AddToGroup("Powerups");
        _applyEffectSound = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        _area = GetNode<Area2D>("Area2D");
        _area.BodyEntered += OnBodyEntered;
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        Type = GetRandomPowerupType();
    }

    public override void _PhysicsProcess(double delta) {
        Velocity = new Vector2(Velocity.X, Velocity.Y + (GetGravity().Y * (float)delta));
        MoveAndSlide();
    }

    public Node2D Spawn() {
        return (Powerup)Load<PackedScene>("res://Scenes/Objects/powerup.tscn").Instantiate();
    }
    
    private void ApplyEffect(Player player) {
        Callable.From(() => {
            _sprite.Visible = false;
            _collisionShape.Disabled = true;
        }).CallDeferred();
            
        _applyEffectSound.Play();
        
        switch (_typeBacking) {
            case PowerupType.Health:
                player.Health += 10;
                break;
            case PowerupType.Ammo:
                player.RefillAmmo();
                break;
            default:
                break;
        }
        
        _applyEffectSound.Finished += QueueFree;
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

