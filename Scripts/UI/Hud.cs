namespace Ionfall.Scripts.UI;

using Godot;
using System;

using Ionfall.Scripts.Resources;

public partial class Hud : Control {
	public HudData Data = new ();
	private Label _health;
	private Label _ammo;
	
	public override void _Ready() {
		_health = GetNode<Label>("Health");
		_ammo = GetNode<Label>("Ammo");
	}

	public override void _Process(double delta) {
		_health.Text = Data.Health.ToString("N0");
		_ammo.Text = $"{Data.Magazine} / {Data.Ammo}";
	}
}
