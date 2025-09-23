using System;
using Godot;

namespace Ionfall.Scripts;

public class Globals {
    public enum GameDirection {
        L, R, U, D
    }

    public static Tuple<GameDirection, GameDirection> GetGameDirectionTo(Node2D pos, Node2D dest) {
        var x = (dest.GlobalPosition.X < pos.GlobalPosition.X)
            ? GameDirection.L
            : GameDirection.R;
        var y = (dest.GlobalPosition.Y < pos.GlobalPosition.Y)
            ? GameDirection.D
            : GameDirection.U;
        return new Tuple<GameDirection, GameDirection>(x, y);
    }
}