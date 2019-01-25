/* Name: Larry Y.
 * Date: December 30, 2018
 * Desc: Only collide with toy bricks, and allow everything else to phase through. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToyBrickContainment : MonoBehaviour {

	private void Start()
	{
		// Get all the gameobjects in the scene, and if they aren't toy bricks, disable collisions between the
		// brick containment and non-toybrick gameobjects.
		GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		for (int i = 0; i < allObjects.Length; i++)
		{
			if (allObjects[i].tag != "ToyBrick")
			{
				Physics2D.IgnoreCollision(allObjects[i].GetComponent<Collider2D>(), GetComponent<Collider2D>());
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) // Failsafe just incase something else gets spawned in
	{
		if (collision.gameObject.tag != "ToyBrick")
		{
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
		}
	}
}
