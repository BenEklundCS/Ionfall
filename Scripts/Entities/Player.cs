using System.Reflection.Metadata;
using Ionfall.Scripts.Interfaces;
using Ionfall.Scripts.Objects;

namespace Ionfall.Scripts.Entities;

using Godot;
using static Godot.GD;
using System;

public partial class Player : Ally, IControllable {
	// exports
	[Export] public int JumpForce = 500;
	[Export] public int CrouchBulletSpawnYOffset = -15;
	// components
	private AnimatedSprite2D _sprite;
	private readonly Bullet _bulletFactory = new ();

	public override void _Ready() {
	   _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	   base._Ready();
	}
	
	public override void _PhysicsProcess(double delta) {
		HandleAnimation(FindFrameByMouseAngle());
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
		var bullet = (Bullet)_bulletFactory.Spawn();
		bullet.GlobalPosition = (_sprite.Animation == "crouch")
			? GlobalPosition + (GetMouseDirection() * BulletSpawnDistance) - new Vector2(0, CrouchBulletSpawnYOffset)
			: GlobalPosition + (GetMouseDirection() * BulletSpawnDistance);
		bullet.Direction = GetMouseDirection();
		GetTree().CurrentScene.AddChild(bullet);
		ShootSound.Play();
	}
	
	public void Crouch() {
	   _sprite.Play(_sprite.Animation == "crouch" ? "idle" : "crouch");
	}
	
	private void Run(Globals.GameDirection direction) {
		LastDirection = direction;
		var speed = direction == Globals.GameDirection.R ? Speed : -Speed;
		Velocity = new Vector2(speed, Velocity.Y);
		
		if (_sprite.Animation.ToString().Contains("run")) return;
		
		_sprite.Animation = $"run_{FindFrameByMouseAngle()}";
		_sprite.Play();
	}
	
	private void HandleAnimation(int directionIndex)
	{
		if (_sprite.Animation.ToString().Contains("run")) {
			HandleRunAnimation(directionIndex);
		}
		else {
			HandleCrouchOrIdleAnimation(directionIndex);
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
		var frame = (int)Math.Round(angle / 7.5) % 48;
	   
		if (_sprite.FlipH)
		{
			frame = (24 - frame) % 48;
			if (frame < 0) frame += 48;
		}
	   
		return frame;
	}
	
	private Vector2 GetMouseDirection() {
		var mousePosition = GetGlobalMousePosition();
		return (mousePosition - GlobalPosition).Normalized();
	}
}
