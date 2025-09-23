using Ionfall.Scripts.Interfaces;

namespace Ionfall.Scripts.Components;

using Godot;
using System;

public partial class Controller : Node2D {
	private IControllable _controlTarget;
	
	public override void _Ready() {
		_controlTarget = GetNode<IControllable>("Player");
	}
	public override void _Process(double delta) {
		// check that control target is valid
		if (!IsInstanceValid(_controlTarget as Node)) return;
		
		// controls
		if (Input.IsActionJustPressed("Crouch")) {
			_controlTarget.Crouch();
		}
		if (Input.IsActionPressed("Left")) {
			_controlTarget.Left();
		}
		if (Input.IsActionPressed("Right")) {
			_controlTarget.Right();
		}
		if (Input.IsActionJustPressed("Jump")) {
			_controlTarget.Jump();
		}
		if (Input.IsActionJustPressed("Shoot")) {
			_controlTarget.Shoot();
		}
		if (Input.IsActionJustReleased("Left") || Input.IsActionJustReleased("Right")) {
			_controlTarget.ReleasedMove();
		}
	}
}
