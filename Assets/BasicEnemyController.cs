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
    [SerializeField] private float movementVelocity = 1.0f;
    
    [Header("Enemy Attack")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float timeToAttackSinceDetection = 2;
    [SerializeField] private int timesToShowAttackArea = 10;
    [SerializeField] private SphereCollider detectionArea;
    [SerializeField] private GameObject weaponObject;
    [SerializeField] private float dashVelocity = 1.0f;
    private bool attacking = false;

    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            //If enemy is attacking, rotate to look to the player
            this.gameObject.transform.LookAt(playerController.gameObject.transform);
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
        float timeToChangeShowDetection = timeToAttackSinceDetection / 2;
        float timeSinceAttackBegan = 0.0f;

        //Show Attack Zone
        weaponObject.SetActive(true);
        yield return new WaitForSeconds(0.20f);
        //Hide Attack Zone
        weaponObject.SetActive(false);
        yield return new WaitForSeconds(0.20f);
        //Show Attack Zone
        weaponObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        //Hide Attack Zone
        weaponObject.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        //Show Attack Zone
        weaponObject.SetActive(true);
        yield return new WaitForSeconds(0.10f);
        //Hide Attack Zone
        weaponObject.SetActive(false);
        yield return new WaitForSeconds(0.10f);
        //Show Attack Zone
        weaponObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        //Hide Attack Zone
        weaponObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        //Show Attack Zone
        weaponObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        //Hide Attack Zone
        weaponObject.SetActive(false);

        //TODO Dash to the player
        Debug.Log("Enemy Attacking");
        enemyRigidBody.AddForce(this.gameObject.transform.forward * dashVelocity, ForceMode.Impulse);

        attacking = false;
    }
}
