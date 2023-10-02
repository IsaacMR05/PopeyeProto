using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthManager : MonoBehaviour 
{

    [SerializeField] private int healthPoints;
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private float invencibilityTime = 0.2f;
    [SerializeField] private Image healthBar;


    [SerializeField] private bool isPlayer;
    private bool isInvencible;


    private void Update()
    {
        if(Keyboard.current.spaceKey.IsPressed())
            DealDamage(5);
    }

    public void DealDamage(int damagePoints)
    {
        if (isInvencible) return;
        healthPoints -= damagePoints;
        if (healthPoints <= 0)
        {
            Death();
            return;
        }

        if (isPlayer)
        {
            healthBar.fillAmount = (float)healthPoints / (float)maxHealthPoints;
        }

        StartCoroutine(Invencibility());
    }

    public void Heal(int healPoints)
    {
        healthPoints += healPoints;
        if(healthPoints > maxHealthPoints) healthPoints = maxHealthPoints;
        if (isPlayer)
        {
            healthBar.fillAmount = (float)healthPoints / (float)maxHealthPoints;
        }
    }

    public void Death()
    {
        if(isPlayer) //Reset Scene
        {
            SceneManager.LoadScene("Gym");
        }
        else //Destroy Enemy
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Invencibility()
    {
        isInvencible = true;
        yield return new WaitForSeconds(invencibilityTime);
        isInvencible = false;
    }
}
