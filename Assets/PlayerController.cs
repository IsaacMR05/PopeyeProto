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
    [SerializeField] private float currentMovementForce = 1.0f;
    [SerializeField] private float movementForceWithAnchor = 1.0f;
    [SerializeField] private float movementForceWithoutAnchor = 1.25f;


    [Header("Player Attack")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private GameObject weaponObject;
    [SerializeField] private BoxCollider attackCollider;
    [SerializeField] private float timeToShowAttackZone = 0.5f;
    private bool attacking = false;
    private bool hasAnchor = true;

    [Header("Heavy Attack")] 
    [SerializeField] private GameObject heavyAttackRangeIndicator;
    [SerializeField] private GameObject anchorObject;
    [SerializeField] private int heavyAttackDamage = 1;
    [SerializeField] private float maxRange = 20.0f;
    [SerializeField] private float timeToChargeAtMaxRange = 1.5f;
    private bool chargingHeavyAttack = false;
    private bool arrivedMaxRange = false;
    private float timeCharging = 0.0f;

    public void Update()
    {
        Vector2 movementInput = Gamepad.current.leftStick.ReadValue() * -1;
        Vector3 forceToApply = new Vector3(movementInput.y * -1, 0.0f, movementInput.x) * currentMovementForce;
        playerRigidBody.AddForce(forceToApply, ForceMode.VelocityChange);
        
        //Rotate Body to Look to the pointing axis
        playerObject.transform.LookAt(new Vector3(playerObject.transform.position.x + movementInput.x,playerObject.transform.position.y,playerObject.transform.position.z + movementInput.y));

        if (Gamepad.current.leftTrigger.IsPressed() && !arrivedMaxRange && hasAnchor)
        {
            //Heavy Attack
            chargingHeavyAttack = true;
            timeCharging += Time.deltaTime;
            Debug.Log("Charging Heavy Attack");
            float indicatorIncreaser = maxRange / 1.5f;
            heavyAttackRangeIndicator.SetActive(true);
            heavyAttackRangeIndicator.transform.localScale = new Vector3(indicatorIncreaser * timeCharging, 1.0f ,1.0f );
            Debug.Log("Indicator increaser " + indicatorIncreaser + " Time Charging: " + timeCharging);


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
            currentMovementForce = movementForceWithoutAnchor;
            hasAnchor = false;
            chargingHeavyAttack = false;
            arrivedMaxRange = false;

            //Throw the anchor to the looking vector of the player + reset timeCharging
            Vector3 chargedAttackOffset = (this.gameObject.transform.forward * heavyAttackRangeIndicator.transform.localScale.x);
            Debug.Log(chargedAttackOffset);
            Vector3 anchorPoint = this.gameObject.transform.position + chargedAttackOffset;
            timeCharging = 0.0f;
            heavyAttackRangeIndicator.SetActive(false);
            heavyAttackRangeIndicator.transform.localScale = new Vector3(1.0f, 1.0f , 1.0f);
            Instantiate(anchorObject, anchorPoint, Quaternion.identity);
            
        }
    }

    public void GetAnchor()
    {
        hasAnchor = true;
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
