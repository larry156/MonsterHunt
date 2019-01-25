// Boon Boonsiri
// Line of sight for AI
// Sometime in Deceember 2018 probably
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class visionScript : NetworkBehaviour {
    public GameObject villian;

    // Use this for initialization
    void Start () {
		
	}
    // Pre: 
    // Post: Checking if player is in vision collider
    private void OnTriggerStay2D(Collider2D theCollidy)
    {
        if (theCollidy.tag == "Player")
        {
            //Debug.Log("I can sort of see");
            GetComponentInParent<VillianMovement>().Sight(theCollidy.gameObject);

        }

        
    }

    // Mainly meant for stalker entering line of sight
    private void OnTriggerEnter2D(Collider2D theCollidy)
    {
        GetComponentInParent<VillianMovement>().Sight(theCollidy.gameObject);

        if (theCollidy.tag == "Player")
        {

            RaycastHit2D hit = Physics2D.Raycast(GetComponentInParent<Transform>().position, (theCollidy.transform.position - GetComponentInParent<Transform>().position), 100);
            if (hit)
            {
                if (hit.collider.gameObject.GetComponentInParent<OnlineMovement>() != null || hit.collider.gameObject.CompareTag("Player"))
                {
                    GetComponentInParent<VillianAttack>().Stalker();
                }

            }
        }
    }
    // Stalker leave line of sight
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player") GetComponentInParent<VillianAttack>().StalkerLeave();
        
    }

    // Update is called once per frame
    void FixedUpdate () {


	}


}
