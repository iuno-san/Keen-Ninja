using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private Ninja playerMovement; 
    private float cooldownTimer = Mathf.Infinity;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 1f;
    public int attackDamage = 10;

    private void Awake() //pobierz animatora i componenty kodu ninja
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Ninja>();

    }

    private void Update() // atack pod przyciskiem spacji
    {
        if (Input.GetKeyDown(KeyCode.K) && cooldownTimer > attackCooldown && playerMovement.canAttack())
                Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack() // atack, wykrywanie wrogów i obrazenia
    {
        //animacja ataku
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        // wykrywanie wrogów i zasiêg ataku
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        // obrazenia
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<enemy>().TakeDamage(attackDamage);
        } 
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
