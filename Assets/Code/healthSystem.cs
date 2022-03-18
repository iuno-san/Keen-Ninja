using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour 
{
    int health;
    int healthMax;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }
    public int Gethealth()
    {
        return health;
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0) health = 0;
    }
    public void Heal(int healAmount)
    {
        health += healAmount;

        if (health > healthMax) health = healthMax;
    }

}
