using Godot;
using System;

public class Fighter
{
    public string Name;
    public int MaxHP { get; set; }
    public int CurrentHP { get; set; }

    public Fighter(string name, int maxHP)
    {
        Name = name;
        MaxHP = maxHP;
        CurrentHP = maxHP;
    }

    public void RecieveDamage(int amount)
    {
        CurrentHP = Math.Max(0, CurrentHP - amount);
    }

    public void Heal(int amount)
    {
        CurrentHP = Math.Min(MaxHP, CurrentHP + amount);
    }

    public bool IsDead => CurrentHP <= 0;
}
