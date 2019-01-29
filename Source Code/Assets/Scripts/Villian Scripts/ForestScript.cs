// Boon Boonsiri
// Detects when werewolf is in forest and makes changes so he doesn't lag the entire server
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestScript : MonoBehaviour {

    // Detects enter forest
    private void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.tag == "Monster")
        {
            Debug.Log("Monster has entered");

            theCollider.GetComponentInChildren<Hearing>().WerewolfEnter();


        }
    }
    // Detects leaveing the forest
    private void OnTriggerExit2D(Collider2D theCollider)
    {
        if (theCollider.tag == "Monster")
        {

            theCollider.GetComponentInChildren<Hearing>().WerewolfEnter();

        }
    }
}
