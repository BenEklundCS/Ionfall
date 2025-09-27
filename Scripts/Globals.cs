using System;
using Godot;

namespace Ionfall.Scripts;

public static class Globals {
    public enum GameDirection {
        L, R, U, D
    }

    public static Tuple<GameDirection, GameDirection> GetGameDirection(Node2D pos, Node2D dest) {
        var x = (dest.GlobalPosition.X < pos.GlobalPosition.X)
            ? GameDirection.L
            : GameDirection.R;
        var y = (dest.GlobalPosition.Y < pos.GlobalPosition.Y)
            ? GameDirection.U
            : GameDirection.D;
        return new Tuple<GameDirection, GameDirection>(x, y);
    }

    public static Random Random = new ();
}