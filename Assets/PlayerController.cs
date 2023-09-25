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
    [SerializeField] private float currentVelocity = 1.0f;
    [SerializeField] private float velocityWithAnchor = 1.0f;
    [SerializeField] private float velocityWithoutAnchor = 1.25f;
    [SerializeField] private float velocityRetrievingAnchor = 0.75f;


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
    private bool canThrowAnchor = true;
    private float timeCharging = 0.0f;
    private Vector3 anchorPoint;

    public void Update()
    {
        Vector2 movementInput = Gamepad.current.leftStick.ReadValue() * -1;
        Vector3 velocityToApply = new Vector3(movementInput.y * -1, 0.0f, movementInput.x) * currentVelocity;
        playerRigidBody.velocity = velocityToApply;
        //Rotate Body to Look to the pointing axis
        playerObject.transform.LookAt(new Vector3(playerObject.transform.position.x + movementInput.x,playerObject.transform.position.y,playerObject.transform.position.z + movementInput.y));

        if (Gamepad.current.leftTrigger.IsPressed() && !arrivedMaxRange && hasAnchor && canThrowAnchor)
        {
            //Heavy Attack
            SetMobilityRetrievingAnchor();
            chargingHeavyAttack = true;
            timeCharging += Time.deltaTime;
            Debug.Log("Charging Heavy Attack");
            float indicatorIncreaser = maxRange / timeToChargeAtMaxRange;
            heavyAttackRangeIndicator.SetActive(true);
            heavyAttackRangeIndicator.transform.localScale = new Vector3(indicatorIncreaser * timeCharging, 1.0f ,1.0f );
            Debug.Log("Indicator increaser " + indicatorIncreaser + " Time Charging: " + timeCharging);


            if (timeCharging >= timeToChargeAtMaxRange)
            {
                arrivedMaxRange = true;
                timeCharging = 1.5f;
            }
        }
        else if (Gamepad.current.rightTrigger.IsPressed() && !attacking && hasAnchor)
        {
            Debug.Log("attack");
            StartCoroutine(ShowAttackZone());
        }
        else if (chargingHeavyAttack)
        {
            Debug.Log("Throwing Anchor");
            //Stop charging and throw heavy attack
            SetMobilityWithoutAnchor();
            hasAnchor = false;
            chargingHeavyAttack = false;
            arrivedMaxRange = false;

            //Throw the anchor to the looking vector of the player + reset timeCharging
            Vector3 chargedAttackOffset = (this.gameObject.transform.GetChild(0).GetChild(3).transform.right * -heavyAttackRangeIndicator.transform.localScale.x);
            Debug.Log(chargedAttackOffset);
            anchorPoint = this.gameObject.transform.position + chargedAttackOffset;
            timeCharging = 0.0f;
            heavyAttackRangeIndicator.SetActive(false);
            heavyAttackRangeIndicator.transform.localScale = new Vector3(1.0f, 1.0f , 1.0f);
            
            Instantiate(anchorObject, anchorPoint, Quaternion.identity);

        }
    }
    

    public void GetAnchor()
    {
        hasAnchor = true;
        currentVelocity = velocityWithAnchor;
    }

    public void SetMobilityWithAnchor()
    {
        currentVelocity = velocityWithAnchor;
    }
    public void SetMobilityWithoutAnchor()
    {
        currentVelocity = velocityWithoutAnchor;
    }
    public void SetMobilityRetrievingAnchor()
    {
        currentVelocity = velocityRetrievingAnchor;
    }

    public void SetCanThrowAnchor(bool value)
    {
        canThrowAnchor = value;
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
