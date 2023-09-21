using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private int currentPlayerHealth = 3;
    [SerializeField] private const int maxPlayerHealth = 3;


    [Header("Player Movement")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private float movementForce = 1.0f;


    [Header("Player Attack")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private GameObject weaponObject;
    [SerializeField] private BoxCollider attackCollider;
    [SerializeField] private float timeToShowAttackZone = 0.5f;
    private bool attacking = false;
    private bool hasAnchor = true;

    [Header("Heavy Attack")] 
    [SerializeField] private GameObject heavyAttackRangeIndicator;
    [SerializeField] private int heavyAttackDamage = 1;
    [SerializeField] private float maxRange = 20.0f;
    [SerializeField] private float timeToChargeAtMaxRange = 1.5f;
    private bool chargingHeavyAttack = false;
    private bool arrivedMaxRange = false;
    private float timeCharging = 0.0f;

    public void Update()
    {
        Vector2 movementInput = Gamepad.current.leftStick.ReadValue() * -1;
        Vector3 forceToApply = new Vector3(movementInput.y * -1, 0.0f, movementInput.x) * movementForce;
        playerRigidBody.AddForce(forceToApply, ForceMode.VelocityChange);
        
        //Rotate Body to Look to the pointing axis
        playerObject.transform.LookAt(new Vector3(playerObject.transform.position.x + movementInput.x,playerObject.transform.position.y,playerObject.transform.position.z + movementInput.y));

        if (Gamepad.current.leftTrigger.IsPressed() && !arrivedMaxRange)
        {
            //Heavy Attack
            chargingHeavyAttack = true;
            timeCharging += Time.deltaTime;
            Debug.Log("Charging Heavy Attack");

            
            if (timeCharging >= timeToChargeAtMaxRange)
            {
                arrivedMaxRange = true;
                timeCharging = 1.5f;
            }
        }
        else if (chargingHeavyAttack)
        {
            Debug.Log("Throwing Anchor");
            //Stop charging and throw heavy attack
            movementForce = 1.5f;
            hasAnchor = chargingHeavyAttack = false;
            
            //Throw the anchor to the looking vector of the player + reset timeCharging
            float anchorPoint = (maxRange / timeToChargeAtMaxRange) * timeCharging;
            timeCharging = 0.0f;
            heavyAttackRangeIndicator.transform.localScale.Set(anchorPoint, 0.0f , 0.0f);
        }
    }

    public void Attack()
    {
        if (attacking || !hasAnchor) return;
        Debug.Log("attack");
        StartCoroutine(ShowAttackZone());
    }

    public void HeavyAttack()
    {
        Debug.Log("heavyAttack");
    }


    IEnumerator ShowAttackZone()
    {
        attacking = true;
        weaponObject.SetActive(attacking);
        yield return new WaitForSeconds(timeToShowAttackZone);
        attacking = false;
        weaponObject.SetActive(attacking);
    }
}
