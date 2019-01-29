/* Name: Larry Y
 * Date: January 1, 2019
 * Desc: On a map, some objects might be traversable, but will slow down anything that goes through it. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureSlow : MonoBehaviour {

	// Set up variables and a dictionary to keep track of which drag value belongs to which gameobject
	private Dictionary<GameObject, float> dragOriginal;
	public float dragMultiplier;
	public string[] applicableTags; // GameObjects tagged as one of these can be slowed.

	private void Start()
	{
		dragOriginal = new Dictionary<GameObject, float>();
	}

	// When something enters the trigger, change their drag.
	// (Usually drag is increased to slow the object down, but theoretically you can also decrease the drag)
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Check if the collision is from a valid object
		bool isValid = false;
		foreach (string i in applicableTags)
		{
			if (collision.tag == i)
			{
				isValid = true;
			}
		}
		// Monsters are very spastic which can cause some issues with drag being multiplied multiple times. Theoretically if an entry already exists in dragOriginal,
		// then the object has already been modified, so it shouldn't be changed further.
		if (isValid)
		{
			Rigidbody2D colRB = collision.attachedRigidbody;
			try
			{
				if (colRB.drag == dragOriginal[collision.gameObject])
				{
					// Do nothing because there shouldn't be an existing entry.
				}
			}
			catch (KeyNotFoundException)
			{
				dragOriginal[collision.gameObject] = colRB.drag;
				colRB.drag = colRB.drag * dragMultiplier;
				Debug.Log(collision.gameObject.name + " had their drag multiplied by " + dragMultiplier + ", and now has a drag of " + colRB.drag);
			}
		}
	}

	// When the gameobject leaves the trigger, reset their drag and remove them from the dictionary.
	private void OnTriggerExit2D(Collider2D collision)
	{
		bool isValid = false;
		foreach (string i in applicableTags)
		{
			if (collision.tag == i)
			{
				isValid = true;
			}
		}

		if (isValid)
		{
			Rigidbody2D colRB = collision.attachedRigidbody;
			colRB.drag = dragOriginal[collision.gameObject];
			dragOriginal.Remove(collision.gameObject);
			Debug.Log(collision.gameObject.name + " had its drag reset to " + colRB.drag);
		}
	}
}
