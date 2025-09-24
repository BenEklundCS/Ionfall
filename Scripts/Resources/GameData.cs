namespace Ionfall.Scripts.Resources;

public struct GameData(int highScore, int health, int magazine, int ammo) {
    public int HighScore = highScore;
    public int Health = health;
    public int Magazine = magazine;
    public int Ammo = ammo;
}