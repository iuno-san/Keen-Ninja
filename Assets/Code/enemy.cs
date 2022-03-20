using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{ //komponenty
    public Animator anima;
    public int maxHealth = 100;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //anima = GetComponent<Animator>();
    }

    public void TakeDamage(int damage) // animacja zranienia
    {
        currentHealth -= damage;
        anima.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die() // animacja œmierci
    {
        Debug.Log("Enemy Died!");

        anima.SetTrigger("dead");

        GetComponent <Collider2D>().enabled = true;
        //GetComponent<CircleCollider2D>().enabled = false;

        this.enabled = false;

       
        
    }
}