namespace Ionfall.Scripts.Entities;

using Godot;
using System;
using Ionfall.Scripts.Entities;

public partial class Roller : Enemy {
    protected override void EnemyProcess(double delta) {
        MoveToPlayer();
    }
}
