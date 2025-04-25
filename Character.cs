// Character.cs - Clasa de bază pentru toate personajele
using Godot;
using System;

public partial class Character : CharacterBody2D
{
    // Proprietăți pentru sistemul de sănătate
    [Export]
    public int MaxHealth { get; set; }
    [Export]
    public int CurrentHealth { get; set; }
    [Export] 
    public int AttackDamage { get; set; }

    // Starea de moarte
    protected bool isDead = false;

    // Metodă pentru a primi damage
    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        CurrentHealth -= damage;
        GD.Print($"{Name} a primit {damage} damage. Sănătate: {CurrentHealth}/{MaxHealth}");

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
    }

    // Se suprascrie în clasele copil pentru comportament specific
    protected virtual void Die()
    {
        isDead = true;
        GD.Print($"{Name} a murit!");
        // Aici poți adăuga animația de moarte sau altă logică specifică
    }

    // Pentru debugging și UI
    public float GetHealthPercentage()
    {
        return (float)CurrentHealth / MaxHealth;
    }
}