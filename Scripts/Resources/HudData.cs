namespace Ionfall.Scripts.Resources;

public struct HudData(int health, int magazine, int ammo) {
    public int Health = health;
    public int Magazine = magazine;
    public int Ammo = ammo;
}