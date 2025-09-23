using Godot;
using System;

using Ionfall.Scripts.Resources;

public partial class Hud : Control {
	public GameData Data;

	private Label _health;
	
	public override void _Ready() {
		_health = GetNode<Label>("Health");
	}

	public override void _Process(double delta) {
		_health.Text = Data.Health.ToString("N0");
	}
}
