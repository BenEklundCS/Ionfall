using Ionfall.Scripts.Entities;
using Ionfall.Scripts.Interfaces;

namespace Ionfall.Scripts.Components;

using Godot;
using System;

public partial class Controller : Node2D {
	private IControllable _controlTarget;
	
	public override void _Ready() {
		_controlTarget = GetNode<IControllable>("Player");
		ChildEnteredTree += OnChildEnteredTree;
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
		if (Input.IsActionPressed("Shoot")) {
			_controlTarget.Shoot();
		}

		if (Input.IsActionJustPressed("Reload")) {
			_controlTarget.Reload();
		}
		if (Input.IsActionJustReleased("Left") || Input.IsActionJustReleased("Right")) {
			_controlTarget.ReleasedMove();
		}
	}

	private void OnChildEnteredTree(Node node) {
		if (node is IControllable controllable && !IsInstanceValid((Node)_controlTarget)) {
			_controlTarget = controllable;
		}
	}
}
