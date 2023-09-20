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


    public void Update()
    {
        Vector2 movementInput = Gamepad.current.leftStick.ReadValue() * -1;
        Vector3 forceToApply = new Vector3(movementInput.y * -1, 0.0f, movementInput.x) * movementForce;
        playerRigidBody.AddForce(forceToApply, ForceMode.VelocityChange);
        
        //Rotate Body to Look to the pointing axis
        playerObject.transform.LookAt(new Vector3(playerObject.transform.position.x + movementInput.x,playerObject.transform.position.y,playerObject.transform.position.z + movementInput.y));
    }

    public void Attack()
    {
        if (attacking) return;
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
