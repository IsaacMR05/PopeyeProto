using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Anchor : MonoBehaviour
{

    [Header("Anchor")]
    [SerializeField] private float timeToGetAnchor = 1.5f;
    [SerializeField] private int damageToEnemies = 2;
    [SerializeField] private float seconsToDealDamage = 0.25f;

    private float timeGettingAnchor = 0.0f;
    private bool isOnRange = true;
    private bool dealDamage = true;
    private PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StopDealingDamage());
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.leftTrigger.IsPressed() && isOnRange)
        {
            StartCoroutine(RetrieveAnchor(timeToGetAnchor));
        }
        else
        {
            StopCoroutine(RetrieveAnchor(timeToGetAnchor));
            this.gameObject.transform.localScale = new Vector3(1.0f, 2.0f, 1.0f);
            timeGettingAnchor = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //If other Game Object is Player
        {
            player = other.GetComponent<PlayerController>();
            isOnRange = true;
        }
        else if(dealDamage && other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Deal 2 Damage points to enemy");
            other.GetComponent<HealthManager>().DealDamage(damageToEnemies);
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //If other Game Object is Player
        {
            isOnRange = false;
        }
    }

    IEnumerator StopDealingDamage()
    {
        yield return new WaitForSeconds(seconsToDealDamage);
        dealDamage = false;
    }

    IEnumerator RetrieveAnchor(float time) //Does not wait what expected
    {
        do
        {
            timeGettingAnchor += Time.deltaTime;
            float tempVal = timeToGetAnchor - timeGettingAnchor;
            this.gameObject.transform.localScale = new Vector3(tempVal / timeToGetAnchor, tempVal / timeToGetAnchor, tempVal / timeToGetAnchor);
            yield return null;

        } while(timeGettingAnchor < time);
        
        
        //player.GetAnchor();
        timeGettingAnchor = 0.0f;
        Destroy(this.gameObject);

    }
}
