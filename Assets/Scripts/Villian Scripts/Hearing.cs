// Boon Boonsiri
// Hearing for AI
// Sometime in Deceember 2018 probably
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hearing : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in allObjects)
        {
            if (obj.tag != "Player")
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
        }

	}
    // If player is in hearing then hear
    private void OnTriggerStay2D(Collider2D theCollidy)
    {
        if (theCollidy.tag == "Player")
        {
            GetComponentInParent<VillianMovement>().Hearing();
            
        }

    }
    private void OnTriggerEnter2D(Collider2D theCollidy)
    {


    }
    // For wwraith leaving, turns back invisible
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(GetComponentInParent<VillianAttack>().name.Contains("Wraith")) GetComponentInParent<VillianAttack>().WraithLeave();
        }

    }
    // Prevents lag, werewolf makes radius of collider smaller
    public void WerewolfEnter()
    {
        if (GetComponentInParent<VillianAttack>().name.Contains("Werewolf"))
        {
            GetComponent<CircleCollider2D>().radius = 20; // This exist for lag

        }
    }
    public void WerewolfExit()
    {
        if (GetComponentInParent<VillianAttack>().name.Contains("Werewolf"))
        {
            GetComponent<CircleCollider2D>().radius = 30;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
