namespace Ionfall.Scripts.Entities;

using Godot;
using System;

public partial class Ally : Character {
    public override void _Ready() {
        Type = CharacterType.Ally;
        base._Ready();
    }
}
