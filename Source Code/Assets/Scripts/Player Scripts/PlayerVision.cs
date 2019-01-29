/* Name: Larry Y.
 * Date: January 15, 2019
 * Desc: If a villain enters the player's line of sight, they will get a "DANGER!" notification on their HUD.
 *		 To make this actually useful, the player's line of sight will be further than the camera displays. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour {

	public GameObject dangerNotifier;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Monster")
		{
			dangerNotifier.SetActive(true);
		}
	}

	// When the player's long vision sweeps past a villain, OnTriggerExit2D is called even if the villain is close enough
	// that the player can "see" it. This prevents the notification from being disabled until it actually should be.
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Monster" && !dangerNotifier.activeInHierarchy)
		{
			dangerNotifier.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Monster")
		{
			dangerNotifier.SetActive(false);
		}
	}
}
