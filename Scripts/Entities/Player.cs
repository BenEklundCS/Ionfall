using System.Reflection.Metadata;
using Ionfall.Scripts.Components;
using Ionfall.Scripts.Interfaces;
using Ionfall.Scripts.Objects;
using Ionfall.Scripts.Resources;
using Ionfall.Scripts.UI;

namespace Ionfall.Scripts.Entities;

using Godot;
using static Godot.GD;
using System;

public partial class Player : Ally, IControllable, ISpawnable {
	
	// exports
	[Export] public int JumpForce = 500;
	
	// components
	private AnimatedSprite2D _sprite;
	private Hud _hud;
	private const int Frames = 48;
	private int _directionIndex = 0;
	private Vector2 _crouchOffset = new (0, 15);

	public override void _Ready() {
	   _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	   _hud = GetNode<Hud>("Hud");
	   base._Ready();
	}

	public override void _Process(double delta) {
		_hud.Data = new HudData(Health, Gun.Magazine, Gun.Ammo); 
	}

	public override void _PhysicsProcess(double delta) {
		_directionIndex = FindFrameByMouseAngle();
		base._PhysicsProcess(delta);
	}

	public void Left() {
		Run(Globals.GameDirection.L);
	}

	public void Right() {
		Run(Globals.GameDirection.R);
	}
	
	public void ReleasedMove() {
		Velocity = new Vector2(0, Velocity.Y);
		if (!_sprite.Animation.ToString().Contains("run")) return;
		_sprite.Animation = "idle";
		_sprite.Stop();
	}

	public void Jump() {
		if (!IsOnFloor()) return;
		Velocity = new Vector2(Velocity.X, -JumpForce);
	}

	public void Shoot() {
		Gun.BulletSpawnOffset = (_sprite.Animation == "crouch") 
			? _crouchOffset 
			: Vector2.Zero;
		Gun.Shoot(GetMouseDirection());
	}

	public void Reload() {
		Gun.BeginReload();
	}

	public void Crouch() {
	   _sprite.Play(_sprite.Animation == "crouch" ? "idle" : "crouch");
	}

	public Node2D Spawn() {
		return (Node2D)Load<PackedScene>("res://Scenes/Entities/player.tscn").Instantiate();
	}
	
	private void Run(Globals.GameDirection direction) {
		LastDirection = direction;
		var speed = direction == Globals.GameDirection.R ? Speed : -Speed;
		Velocity = new Vector2(speed, Velocity.Y);
		
		if (_sprite.Animation.ToString().Contains("run")) return;
		
		_sprite.Animation = $"run_{FindFrameByMouseAngle()}";
		_sprite.Play();
	}
	
	protected override void HandleAnimation()
	{
		if (_sprite.Animation.ToString().Contains("run")) {
			HandleRunAnimation(_directionIndex);
		}
		else {
			HandleCrouchOrIdleAnimation(_directionIndex);
		}
	}

	private void HandleRunAnimation(int directionIndex) {
		var run = $"run_{directionIndex}";
		if (_sprite.Animation != run) {
			var oldFrame = _sprite.Frame;
			var oldProgress = _sprite.FrameProgress;
			
			_sprite.Animation = run;
			_sprite.Play(); 
			
			_sprite.Frame = oldFrame;
			_sprite.FrameProgress = oldProgress;

		}
		else {
			if (!_sprite.IsPlaying()) _sprite.Play();
		}
	}
	private void HandleCrouchOrIdleAnimation(int directionIndex)
	{
		var pose = _sprite.Animation == "crouch" ? "crouch" : "idle";
		if (_sprite.Animation != pose)
		{
			_sprite.Animation = pose;
			_sprite.Stop();
		}
		_sprite.Frame = directionIndex;
	}
	
	private int FindFrameByMouseAngle() {
		var dir = GetMouseDirection();
		// Flip angle direction
		var angle = -Mathf.RadToDeg(dir.Angle());
		// Map (-180,180) → (0,360)
		if (angle < 0) angle += 360;
		// 48 frames in the circle → 7.5° per frame
		var frame = (int)Math.Round(angle / (360.0f / Frames)) % Frames;
	   
		if (_sprite.FlipH)
		{
			frame = ((Frames/2) - frame) % Frames;
			if (frame < 0) frame += Frames;
		}
	   
		return frame;
	}
	
	private Vector2 GetMouseDirection() {
		var mousePosition = GetGlobalMousePosition();
		return (mousePosition - GlobalPosition).Normalized();
	}
}
