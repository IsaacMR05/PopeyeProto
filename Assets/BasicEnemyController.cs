using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    
    [Header("Enemy Health")]
    [SerializeField] private int currentEnemyHealth = 3;
    [SerializeField] private const int maxEnemyHealth = 3;
    
    [Header("Enemy Movement")]
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private Rigidbody enemyRigidBody;
    [SerializeField] private float movementForce = 1.0f;
    
    [Header("Enemy Attack")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float timeToAttackSinceDetection = 1;
    [SerializeField] private SphereCollider detectionArea;
    [SerializeField] private GameObject weaponObject;
    private bool attacking = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            //If enemy is attacking, rotate to look to the player
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !attacking) //If Detects Player
        {
            //Start Attack
            StartCoroutine(Attack());
            Debug.Log("Enemy Attacking");
        }
    }
    
    IEnumerator Attack()
    {
        attacking = true;
        float timeSinceAttackBegan = 0.0f;
        float  timeWhenAttackBegan = Time.deltaTime; //Const
        do
        {
            //Show Attack Zone
            weaponObject.SetActive(true);
            Debug.Log("Showing Area Attack");
            timeSinceAttackBegan = Time.deltaTime - timeWhenAttackBegan;
            float timeToWait = (timeToAttackSinceDetection / (timeSinceAttackBegan * 10));
            yield return new WaitForSeconds(timeToWait);
            
            //Hide Attack Zone
            weaponObject.SetActive(false);
            Debug.Log("Unshowing Area Attack");
        } while (timeSinceAttackBegan < timeToAttackSinceDetection);
        attacking = false;
    }
}
