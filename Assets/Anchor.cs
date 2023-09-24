using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Anchor : MonoBehaviour
{

    [Header("Anchor")]
    [SerializeField] private float timeToGetAnchor = 1.5f;
    [SerializeField] private float damageToEnemies = 2.0f;

    private float timeGettingAnchor = 0.0f;
    private bool isOnRange = true;
    private PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current.leftTrigger.IsPressed() && isOnRange)
        {
            Debug.Log("Starts Getting Anchor");
            timeGettingAnchor += Time.deltaTime;
            float tempVal = timeToGetAnchor - timeGettingAnchor;
            this.gameObject.transform.localScale = new Vector3(tempVal / timeToGetAnchor, tempVal / timeToGetAnchor, tempVal / timeToGetAnchor);

            if (timeGettingAnchor >= timeToGetAnchor)
            {
                //Player Gets Anchor
                player.GetAnchor();
                Destroy(this.gameObject);

            }
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(1.0f, 2.0f, 1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (other.gameObject.CompareTag("Player")) //If other Game Object is Player
        {
            isOnRange = true;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (other.gameObject.CompareTag("Player")) //If other Game Object is Player
        {
            isOnRange = false;
        }
    }
}
