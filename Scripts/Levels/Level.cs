namespace Ionfall.Scripts.Levels;

using Ionfall.Scripts.Entities;
using Ionfall.Scripts.Resources;

using Godot;
using System;

public partial class Level : Node2D {
    private Hud _hud;
    private Player _player;
    private GameData _liveData = new ();
	
    public override void _EnterTree()                                                                                                                                                                                             
    {             
        _player = GetNode<Player>("Controller/Player");        
        ChildEnteredTree += OnChildEnteredTree;                                                                                                                                                                                   
        ChildExitingTree += OnChildExitingTree;  
    }    
    
    public override void _Ready() {
        _liveData.HighScore = 0;
        _hud = GetNode<Hud>("Controller/Player/Hud");
    }
    
    public override void _ExitTree()                                                                                                                                                                                              
    {                                                                                                                                                                                                                             
	    ChildEnteredTree -= OnChildEnteredTree;                                                                                                                                                                                   
	    ChildExitingTree -= OnChildExitingTree;                                                                                                                                                                                   
    }      
    
    public override void _Process(double delta) {
        _liveData.Health = _player.Health;
        _hud.Data = _liveData;
    }
    
    private void OnCharacterDeath(Character character) {
	    _liveData.HighScore += character.Score;
    }      
    
    private void OnChildEnteredTree(Node node) {                                                                                                                                                                                                                             
		node.ChildEnteredTree += OnChildEnteredTree;                                                                                                                                                                              
		node.ChildExitingTree += OnChildExitingTree;                                                                                                                                                                              
																																																								
		if (node is Character character)                                                                                                                                                                                          
		  character.OnDeath += OnCharacterDeath;
		if (node is Enemy enemy)
		  enemy.TrackedPlayer = _player;
	}                 
	  
	private void OnChildExitingTree(Node node) {                                                                                                                                                                                                                             
	  node.ChildEnteredTree -= OnChildEnteredTree;                                                                                                                                                                              
	  node.ChildExitingTree -= OnChildExitingTree;

	  if (node is Character character)
		  character.OnDeath -= OnCharacterDeath;
	}     
}
