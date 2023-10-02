using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float timeBetweenBullets;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private GameObject bullet;
    void Start()
    {
        StartCoroutine(BulletSpawn());
    }

    IEnumerator BulletSpawn()
    {
        Debug.Log("Spawning Bullet");
        GameObject newbullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
        newbullet.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * bulletVelocity;
        yield return new WaitForSeconds(timeBetweenBullets);
        StartCoroutine(BulletSpawn());
    }
}
