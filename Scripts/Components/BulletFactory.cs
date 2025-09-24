using System.Linq;
using Ionfall.Scripts.Interfaces;

namespace Ionfall.Scripts.Components;

using Godot;
using System;
using Godot.Collections;

using Ionfall.Scripts.Objects;

public partial class BulletFactory : Node2D {
    [Export] public Dictionary<string, Texture2D> BulletTextures;
    private readonly Bullet _bulletFactory = new ();

    public Bullet GetBullet(string bulletName) {
        var bullet = (Bullet)_bulletFactory.Spawn();
        // guard, return first valid bullet if not found
        if (!BulletTextures.ContainsKey(bulletName)) bulletName = BulletTextures.Keys.First();
        bullet.GetNode<Sprite2D>("Sprite2D").Texture = BulletTextures[bulletName];
        return bullet;
    }
}