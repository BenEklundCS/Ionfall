using Godot;
using System;
using System.Collections.Generic;

public partial class ParallaxBackground : Godot.ParallaxBackground {
    
    [Export] public Godot.Collections.Dictionary<Texture2D, float> Layers = new ();
    [Export] public CharacterBody2D TrackedPlayer;
    [Export] public Vector2 SpriteSpawnPosition = new Vector2(0, 250);
    [Export] public Vector2 SpriteScale = new Vector2(3, 3);
    [Export] public int RegionWidth = 10000;
    [Export] public float BaseMotionScale = 0.1f;
    public override void _Ready() {
        ScrollBaseScale = new Vector2(1, 1);
        foreach (var layer in Layers) {
            AddChild(GetLayer(layer));
        }
    }

    private ParallaxLayer GetLayer(KeyValuePair<Texture2D, float> pair) {
        var parallaxLayer = new ParallaxLayer();
        parallaxLayer.AddChild(GetSprite(pair.Key));
        parallaxLayer.MotionScale = new Vector2(pair.Value * BaseMotionScale, 0);
        return parallaxLayer;
    }

    private Sprite2D GetSprite(Texture2D texture) {
        var sprite = new Sprite2D();
        sprite.Texture = texture;
        sprite.TextureRepeat = CanvasItem.TextureRepeatEnum.Enabled;
        sprite.RegionEnabled = true;
        sprite.RegionRect = new Rect2(sprite.RegionRect.Position, new Vector2(RegionWidth, RegionWidth));
        sprite.Scale = SpriteScale;
        sprite.GlobalPosition = SpriteSpawnPosition;
        return sprite;
    }
}