/* Name: Larry Y.
 * Date: December 16, 2018
 * Desc: If a player collides with an object that has this script, they might trip or stub their toe. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureStun : MonoBehaviour {

	public int tripChance;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Someone ran into me");
		// This should only run if the collision is from a player.
		if (collision.gameObject.tag == "Player")
		{
			int tripRoll = Random.Range(0, 100); // Random number from 0 to 99
			Debug.Log("Trip Chance: " + tripChance + " Trip Roll: " + tripRoll);
			if (tripRoll < tripChance)
			{
				// There shouldn't be any need to fiddle around with Commands and RPCs since only the local player is getting stunned.
				// This doesn't use CallStunFunc() because that function is for players stunning each other.
				OnlineMovement playerScript = collision.gameObject.GetComponent<OnlineMovement>();
				if (!playerScript.isStunned)
				{
					Debug.Log("You stubbed your toe and need a moment to recover.");
					playerScript.isStunned = true;
					playerScript.stunnedAt = Time.time;
					playerScript.Invoke("UnstunMe", playerScript.stunDuration);
				}
			}
		}
	}
}
