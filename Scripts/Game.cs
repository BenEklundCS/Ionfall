using Ionfall.Scripts.Components;


namespace Ionfall.Scripts;

using Godot;
using static Godot.GD;

public partial class Game : Node2D {

	private BulletFactory _bulletFactory;
	
	public override void _Ready() {
		_bulletFactory = GetNode<BulletFactory>("/root/BulletFactory");
	}
} 
