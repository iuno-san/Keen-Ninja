using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]

    [SerializeField] public float startingHealth;
    public bool IsDead { get { return currentHealth == 0; } }
    public float currentHealth { get; private set; }  //jest publiczne ale nie mozna zmieniac wartosci
    private Animator anim;

    [Header("iFrames")]

    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Death && hurt Sound")]
    [SerializeField] private AudioClip DeathSound;
    [SerializeField] private AudioClip HurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //player hurt
            anim.SetTrigger("hurt");
           /// SoundManager.Instance.PlaySound(HurtSound);

            //ifFrames
            StartCoroutine(Invunerabillity());
        }
        else
        {
            //player dead
            anim.SetTrigger("dead");
            SoundManager.Instance.PlaySound(DeathSound);
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerabillity()
    {
        //invunerabillity duration
        Physics2D.IgnoreLayerCollision(10, 9, true);
        for(int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 9, false);

    }

    private void Deactivate()
    {
      gameObject.SetActive(false);
    }

}
