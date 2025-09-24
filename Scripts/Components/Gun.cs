using System;
using Ionfall.Scripts.Entities;
using Ionfall.Scripts.Interfaces;

namespace Ionfall.Scripts.Components;

using Godot;
using static Godot.GD;

public partial class Gun : Node2D, IGun {

    private Timer _reloadTimer;
    private Timer _cooldownTimer;
    private bool _onCooldown;
    
    [Export] public string BulletName = "blue";
    [Export] public AudioStream ShootSoundEffect;
    [Export] public float ReloadTime;
    [Export] public float Cooldown;
    [Export] public Character.CharacterType Target;
    [Export] public bool ReloadEnabled = false;
    
    [Export] public int AmmoCapacity = 100;
    private int _ammo;
    public int Ammo {
        get => _ammo;
        set {
            value = Math.Clamp(value, 0, AmmoCapacity);
            _ammo = value;
        }
    }
    
    [Export] public int MagazineCapacity = 30;
    private int _magazine;
    public int Magazine {
        get => _magazine;
        set {
            value = Math.Clamp(value, 0, MagazineCapacity);
            _magazine = value;
        }
    }

    [Export] public int BulletSpawnDistance = 50;
    [Export] public Vector2 BulletSpawnOffset = new (50, 0);

    private AudioStreamPlayer2D _shootSound;

    public override void _Ready() {
        Magazine = MagazineCapacity;
        Ammo = AmmoCapacity;
        _shootSound = GetNode<AudioStreamPlayer2D>("ShootSound");
        
        _reloadTimer = GetNode<Timer>("ReloadTimer");
        _reloadTimer.Timeout += OnReloadTimerTimeout;

        _cooldownTimer = GetNode<Timer>("CooldownTimer");
        _cooldownTimer.Timeout += OnCooldownTimerTimeout;
        
        if (ShootSoundEffect != null) 
			_shootSound.Stream = ShootSoundEffect;
    }

    public void Shoot(Vector2 direction) {
        if (_onCooldown) return;
        
        if (!ReloadEnabled) {
            Fire(direction);
            return;
        }
        
        if (Magazine > 0) {
            Fire(direction);
            Magazine -= 1;
        }
        else {
            if (Ammo > 0) {
                if (_reloadTimer.IsStopped())
                    _reloadTimer.Start();
                Print("Reloading!");
            }
            else {
                Print("Out of ammo!");
            }
        }
    }

    private void Fire(Vector2 direction) {
        var bullet = GetNode<BulletFactory>("/root/BulletFactory").GetBullet(BulletName);
        bullet.GlobalPosition = GlobalPosition + (direction * BulletSpawnDistance) + BulletSpawnOffset;
        bullet.Direction = direction;
        bullet.Target = Target;
        GetTree().CurrentScene.AddChild(bullet);
        _shootSound.Play();
        OnCooldown();
    }

    private void Reload() {
        var total = Ammo + Magazine;
        Magazine = Math.Min(MagazineCapacity, total);
        Ammo = total - Magazine;
    }

    private void OnReloadTimerTimeout() {
        Reload();
    }

    private void OnCooldown() {
        if (!_cooldownTimer.IsStopped()) return;

        _cooldownTimer.Start();
        _onCooldown = true;

    }

    private void OnCooldownTimerTimeout() {
        _onCooldown = false;
    }
}