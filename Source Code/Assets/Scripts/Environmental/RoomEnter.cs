/* Name: Larry Y.
 * Date: December 5, 2018
 * Desc: When a player enters a room, the roof is invisible. When they leave, it appears again. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnter : MonoBehaviour {

	public GameObject roofObject;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var playerScript = collision.GetComponent<OnlineMovement>();
		// If playerScript isn't null, then this must be an online player
		if (playerScript != null)
		{
			if (playerScript.IsControlledPlayer())
			{
				roofObject.SetActive(false);
			}
		}
		// So this still works with the offline player I use for testing
		else if (collision.GetComponent<BasicMovement>() != null)
		{
			if (collision.tag == "Player")
			{
				roofObject.SetActive(false);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		var playerScript = collision.GetComponent<OnlineMovement>();
		if (playerScript != null)
		{
			if (playerScript.IsControlledPlayer())
			{
				roofObject.SetActive(true);
			}
		}
		else if (collision.GetComponent<BasicMovement>() != null)
		{
			if (collision.tag == "Player")
			{
				roofObject.SetActive(true);
			}
		}
	}
}
