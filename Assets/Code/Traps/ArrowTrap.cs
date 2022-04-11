using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject arrow;
    private float cooldownTimer;

    [Header("SFX")]
    [SerializeField] private AudioClip ArrowSound;
    public void ShootArrow()
    {
        SoundManager.Instance.PlaySound(ArrowSound);
        Instantiate(
            arrow, 
            firePoint.transform.position, 
            firePoint.transform.rotation
        );
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;

            ShootArrow();
        }
    }

}
