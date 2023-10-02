using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private bool dealDamageToEnemies = false;
    [SerializeField] private int bulletDamage;
    [SerializeField] private float timeUntilAutodestroy;

    private void Start()
    {
        StartCoroutine(DestroyAfterLifeTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Enemy") && dealDamageToEnemies)) //If collides with player
        {
            collision.gameObject.GetComponent<HealthManager>().DealDamage(bulletDamage);
            Destroy(this.gameObject);
        }
    }

    IEnumerator DestroyAfterLifeTime()
    {
        yield return new WaitForSeconds(timeUntilAutodestroy);
        Destroy(this.gameObject);
    }
}
