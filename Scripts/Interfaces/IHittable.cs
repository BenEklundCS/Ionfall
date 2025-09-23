namespace Ionfall.Scripts.Interfaces;

using Ionfall.Scripts.Objects;

public interface IHittable {
    public void OnHit(Bullet bullet);
}