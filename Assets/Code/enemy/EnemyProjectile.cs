using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage //Will damage the player every time they touch
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifeTime;
    void Start()
    {
        lifeTime = 0;
    }

    private void Update()
    {
        float  movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifeTime += Time.deltaTime;
        if (lifeTime >= resetTime)
            DestroyArrow();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        DestroyArrow();
    }


    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
