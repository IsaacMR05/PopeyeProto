using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
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

    public void Move(InputAction.CallbackContext context)
    {

        Debug.Log("move");
        Vector2 movementInput = context.ReadValue<Vector2>();

        Vector3 forceToApplye = new Vector3(movementInput.x, 0.0f, movementInput.y) * movementForce;
        playerRigidBody.AddForce(movementInput * movementForce);

        return;
    }

    public void Attack()
    {
        Debug.Log("attack");

    }

    public void HeavyAttack()
    {
        Debug.Log("heavyAttack");
    }
}
