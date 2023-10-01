using System;
using System.Collections;
using System.Collections.Generic;using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public enum AnchorState
{
    WITH_PLAYER,
    THROWING,
    RETRIEVING,
    ON_GROUND
};

public class NewAnchor : MonoBehaviour
{

    [Header("General")] 
    [SerializeField] private Rigidbody anchorRB;
    [FormerlySerializedAs("maxRange")] 
    [SerializeField] private float maxRangeWithPlayer = 1.0f;
    [SerializeField] private float maxRangeThrowing = 1.0f;
    [SerializeField] private int damageToDeal = 1;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Transform anchorTransformWithPlayer;
    //[SerializeField] private Vector3 anchorRotationThrowing; -90X
    //[SerializeField] private Vector3 anchorRotationRetrieving; 90X
    private Vector3 anchorDesiredPosition;
    private Vector3 anchorInitPosition;
    private AnchorState anchorState = AnchorState.WITH_PLAYER; //Change to WITH_PLAYER when not debugging
    private Vector3 previousPlayerPosition;
    private bool canRetrieve = true;
    private bool canThrow = true;

    
    [Header("WTF")]
    [SerializeField] private LineRenderer anchorRope;
    [SerializeField] private Color lineRendererColor;
    [SerializeField] private float lineRendererWidth = 1.0f;
    
    [Header("Retrieve")]
    [SerializeField] private float retrieveKnockBackForce = 1.0f;
    [SerializeField] private float retrieveTimeToMaxRange = 1.0f;
    
    [Header("Throw")]
    [SerializeField] private float throwKnockBackForce = 1.0f;
    [SerializeField] private float throwTimeToMaxRange = 1.0f;



    void Start()
    {
        anchorRope.endWidth = lineRendererWidth;
        anchorRope.startWidth = lineRendererWidth;
        anchorRope.startColor = lineRendererColor;
        anchorRope.endColor = lineRendererColor;
        anchorRope.useWorldSpace = true;
        previousPlayerPosition = playerTransform.position;
    }

    void Update()
    {
        
        //Calculate if player is in range
        float distancePlayerToAnchor = Vector3.Distance(this.gameObject.transform.position,
            playerTransform.position);
        if (distancePlayerToAnchor >= maxRangeWithPlayer) //if not, set him previous position
        {
            lineRendererColor = Color.red;
            playerTransform.position = previousPlayerPosition; //May do some clipping
        }
        else if (distancePlayerToAnchor >= maxRangeWithPlayer / 2)
        {
            lineRendererColor = Color.yellow;
        }
        else
        {
            lineRendererColor = Color.green;
        }

        anchorRope.startColor = lineRendererColor;
        anchorRope.endColor = lineRendererColor;
        anchorRope.SetPosition(0,this.gameObject.transform.position);
        anchorRope.SetPosition(1,playerTransform.position);
        switch (anchorState)
        {
            case AnchorState.ON_GROUND:
            {
                
                break;
            }


            case AnchorState.THROWING:
            {
                if (Vector3.Distance(this.gameObject.transform.position, anchorInitPosition) > maxRangeThrowing || this.gameObject.transform.position == anchorDesiredPosition) //Anchor Has arrived to destination
                {
                    Debug.Log("Throw Arrived to max distance");
                    //Set state of Anchor to On_Ground
                    anchorState = AnchorState.ON_GROUND;
                    
                    //Reset Velocity
                    anchorRB.velocity = Vector3.zero;
                    
                    //Deal Damage //TODO
                }
                break;
            }
            case AnchorState.WITH_PLAYER:
            {
                this.gameObject.transform.position = anchorTransformWithPlayer.position;
                break;
            }
            case AnchorState.RETRIEVING:
            { 
                //If anchor is close enough to player, set anchor state to WITH_PLAYER and reset velocity
                if (Vector3.Distance(this.transform.position, playerTransform.position) < 1.0f)
                {
                    anchorState = AnchorState.WITH_PLAYER;
                    anchorRB.velocity = Vector3.zero;
                }
                else //Update anchor velocity to go to the playey
                {
                    //Calculate the velocity to retrieve anchor to its max distance in wanted time (x = initial x + velocity * time) --> velocity = (x - initial x) / time
                    Vector3 finalPos = ((bodyTransform.position - this.gameObject.transform.position) * maxRangeThrowing) + bodyTransform.position;
                    Vector3 newVelocity = (finalPos - this.gameObject.transform.position) / retrieveTimeToMaxRange;
                    anchorRB.velocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);
                    anchorDesiredPosition = finalPos;
                }
                break;
            }
        }
        
        //Update Previous Player Position with current Position
        previousPlayerPosition = playerTransform.position;
        
        //Input
        if (Gamepad.current.rightTrigger.IsPressed()) //If player decides to do an attack
        {
            switch (anchorState)
            {
                case AnchorState.WITH_PLAYER: //The anchor is with the player --> Will throw its anchor
                {
                    if (!canThrow) return;
                    canRetrieve = false;
                    canThrow = false;
                    //Calculate the velocity to throw anchor to its max distance in wanted time (x = initial x + velocity * time) --> velocity = (x - initial x) / time
                    Vector3 finalPos = (bodyTransform.forward * maxRangeThrowing) + bodyTransform.position;
                    Vector3 newVelocity = (finalPos - this.gameObject.transform.position) / throwTimeToMaxRange;
                    anchorRB.velocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);
                    
                    //Set anchor state to throwing
                    anchorState = AnchorState.THROWING;
                    anchorDesiredPosition = finalPos;
                    anchorInitPosition = this.gameObject.transform.position;
                    Debug.Log("Throw Anchor with velocity" + newVelocity);
                    break;
                }
                case AnchorState.ON_GROUND:
                case AnchorState.THROWING: //Not Working properly //TODO
                { //Start Retrieving
                    if (!canRetrieve) return;
                    //Calculate the velocity to retrieve anchor to its max distance in wanted time (x = initial x + velocity * time) --> velocity = (x - initial x) / time
                    Vector3 finalPos = ((bodyTransform.position - this.gameObject.transform.position) * maxRangeThrowing) + bodyTransform.position;
                    Vector3 newVelocity = (finalPos - this.gameObject.transform.position) / retrieveTimeToMaxRange;
                    anchorRB.velocity = new Vector3(newVelocity.x, 0.0f, newVelocity.z);
                    
                    //Set anchor state to retrieving
                    anchorState = AnchorState.RETRIEVING;
                    anchorDesiredPosition = finalPos;
                    anchorInitPosition = this.gameObject.transform.position;
                    Debug.Log("Retrieve Anchor with velocity" + newVelocity);
                    break;
                }
                    
            }
        }
        else
        {
            canRetrieve = true;
            if(anchorState == AnchorState.WITH_PLAYER)
                canThrow = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) //If Enemy enters to trigger
        {
            Rigidbody enemyRB = other.gameObject.GetComponent<Rigidbody>();
   
            //Check in which state anchor is
            switch (anchorState)
            {
                case AnchorState.THROWING:
                {
                    //Add knockback to the enemy
                    enemyRB.AddForce(anchorRB.velocity.normalized * throwKnockBackForce, ForceMode.Impulse);
                    Debug.Log("Knockbacked enemy");
                    
                    //Deal Damage
                    break;
                }
                case AnchorState.RETRIEVING:
                {
                    //Add knockback to the enemy
                    enemyRB.AddForce(anchorRB.velocity.normalized * retrieveKnockBackForce, ForceMode.Impulse);
                    Debug.Log("Knockbacked enemy");
                    //Deal Damage
                    break;
                }
            }
        }
    }
}
