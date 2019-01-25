/* Name: Larry Y.
 * Date: January 11, 2019
 * Desc: Quick script to disable collisions for ghost villains */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GhostCollision : NetworkBehaviour
{
	private bool hasInitialized = false;

	// Get all gameobjects and disable collisions between the ghost and them if they aren't a map boundary or player.
	private void Start()
	{
		Debug.Log("Disabling collisions.");
		GameObject[] gameObjects = FindObjectsOfType<GameObject>();
		foreach (GameObject obj in gameObjects)
		{
			if (obj.tag != "Boundary" && !obj.tag.Contains("Player"))
			{
				//Debug.Log(obj.name + " will not collide with me");
				Physics2D.IgnoreCollision(GetComponent<Collider2D>(), obj.GetComponent<Collider2D>());
			}
		}
	}
}
