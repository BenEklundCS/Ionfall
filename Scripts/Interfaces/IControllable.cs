namespace Ionfall.Scripts.Interfaces;

using Godot;
using System;

public interface IControllable {
    public void Left();
    public void Right();
    public void Jump();
    public void Shoot();
    public void Crouch();
    public void ReleasedMove();
}
