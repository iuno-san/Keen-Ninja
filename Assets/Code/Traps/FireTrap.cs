using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float Damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    [Header("SFX")]
    [SerializeField] private AudioClip FireTrapSound;

    private bool triggered = false; //kiedy pu³apka zostanie uruchomiona
    private bool active = true;   //gdy pu³apka jest aktywna i mo¿e zraniæ gracza
    private void Awake()
    {
         anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
                StartCoroutine(ActivateFireTrap());    //uruchomienie pu³apki ogniowej

            if (active)
                collision.GetComponent<Health>().TakeDamage(Damage);
        }
    }
    private IEnumerator ActivateFireTrap()
    {
        triggered = true;
        //active = false;
        //spriteRend.color = Color.red;  //zmienia kolor sprite'a na czerwony, aby powiadomiæ gracza

        //czekanie na opuznienie ,aktywowanie ognistej pu³apki, w³¹czenie animacji i ustawienie coloru na standardowy
        yield return new WaitForSeconds(activationDelay);
        SoundManager.Instance.PlaySound(FireTrapSound);
        spriteRend.color = Color.white;  //przywraca sprite'owi jego pocz¹tkowy kolor
        active = true;
        //anim.SetBool("Activate", true); 
    }
}
