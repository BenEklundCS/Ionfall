using Ionfall.Scripts.Components;
using Ionfall.Scripts.UI;

namespace Ionfall.Scripts.Levels;

using Ionfall.Scripts.Entities;
using Ionfall.Scripts.Resources;

using Godot;
using System;

public partial class Level : Node2D {
	private Controller _controller;
	private Player _player;
	private Player _playerFactory = new ();
	private Timer _respawnTimer;
	private Vector2 _respawnPosition = Vector2.Zero;
	
	public override void _EnterTree() {
		ChildEnteredTree += OnChildEnteredTree;                                                                                                                                                                                   
		ChildExitingTree += OnChildExitingTree;  
	}    
	
	public override void _Ready() {
		_controller = GetNode<Controller>("Controller");
		_player = GetNode<Player>("Controller/Player");
		_respawnTimer = GetNode<Timer>("RespawnTimer");
		_respawnTimer.Timeout += OnRespawnTimerTimeout;
	}
	
	public override void _ExitTree() {                                                                                                                                                                                                                             
		ChildEnteredTree -= OnChildEnteredTree;                                                                                                                                                                                   
		ChildExitingTree -= OnChildExitingTree;                                                                                                                                                                                   
	}      
	
	private void OnChildEnteredTree(Node node) {                                                                                                                                                                                                                             
		node.ChildEnteredTree += OnChildEnteredTree;                                                                                                                                                                              
		node.ChildExitingTree += OnChildExitingTree;                                                                                                                                                                              
		RegisterNodeListeners(node);
	}                 
	
	private void OnChildExitingTree(Node node) {                                                                                                                                                                                                                             
		node.ChildEnteredTree -= OnChildEnteredTree;                                                                                                                                                                              
		node.ChildExitingTree -= OnChildExitingTree;
		RemoveNodeListeners(node);
	}

	private void RegisterNodeListeners(Node node) {
		if (node is Character character) character.OnDeath += OnCharacterDeath;
		if (node is Enemy enemy) enemy.TrackedPlayer = _player;
		if (node is Player player) {
			_player = player;
			foreach (var n in GetChildren()) {
				if (n is Enemy e)
					e.TrackedPlayer = _player;
			}
		}
	}

	private void RemoveNodeListeners(Node node) {
		if (node is Character character)
			character.OnDeath -= OnCharacterDeath;
	}
	
	private void OnCharacterDeath(Character character) {
		if (character is Player player) {
			_respawnPosition = _player.GlobalPosition;
			_respawnTimer.Start();
		}
	}

	private void OnRespawnTimerTimeout() {
		var player = (Player)_playerFactory.Spawn();
		player.GlobalPosition = _respawnPosition;
		_controller.AddChild(player);
	}
}
