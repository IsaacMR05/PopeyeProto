using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{

    [SerializeField] private int attackDamage = 1;

    private void OnTriggerEnter(Collider other)
    {
        HealthManager otherHealthManager = other.gameObject.GetComponent<HealthManager>();
        otherHealthManager.DealDamage(attackDamage);
    }
}
