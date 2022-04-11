using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerAttackBase : MonoBehaviour
//{
//    ..... ca³a logika
//}

//public class PlayerAttack2 : PlayerAttackBase
//{

//}


public class PlayerAttack2 : MonoBehaviour
{
   [SerializeField] private float attackCooldown;
   [SerializeField] private AudioClip AttackSound;
    private Animator anim2;
    private Ninja playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    public Transform attackPoint2;
    public LayerMask enemyLayers2;
    public float attackRange2 = 1f;
    public int attackDamage2 = 10;

    private void Awake() //pobierz animatora i componenty kodu ninja
    {
        anim2 = GetComponent<Animator>();
        playerMovement = GetComponent<Ninja>();

    }

    private void Update() // atack pod przyciskiem
    {
        if (Input.GetKeyDown(KeyCode.L) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack() // atack, wykrywanie wrogów i obrazenia
    {
        SoundManager.Instance.PlaySound(AttackSound);

        //animacja ataku
        anim2.SetTrigger("attack2");
        cooldownTimer = 0;

        // wykrywanie wrogów i zasiêg ataku
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange2, enemyLayers2);

        // obrazenia
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<enemy>().TakeDamage(attackDamage2); 
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint2 == null)
            return;

        Gizmos.DrawWireSphere(attackPoint2.position, attackRange2);
    }

}
