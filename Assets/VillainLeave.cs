using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillainLeave : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    // Not useful code
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            //collision.GetComponent<VillianMovement>().villianLeave();

        }
    }
}
