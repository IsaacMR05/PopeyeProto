using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour 
{

    [SerializeField] private int healthPoints;
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private float invencibilityTime = 0.2f;


    [SerializeField] private bool isPlayer;
    private bool isInvencible;


    public void DealDamage(int damagePoints)
    {
        if (isInvencible) return;
        healthPoints -= damagePoints;
        if (healthPoints <= 0)
        {
            Death();
            return;
        }

        StartCoroutine(Invencibility());
    }

    public void Heal(int healPoints)
    {
        healthPoints += healPoints;
        if(healthPoints > maxHealthPoints) healthPoints = maxHealthPoints;
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
