using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float startingHealth;
    public float currentHealth { get; private set; }  //jest publiczne ale nie mozna zmieniac wartosci
    private Animator anim;
    public bool isDead { get { return currentHealth == 0; } }
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //player hurt
            anim.SetTrigger("hurt");
            //ifFrames
        }
        else
        {
            //player dead
            anim.SetTrigger("die");
        }
    }

}
