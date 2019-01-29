/* Name: Larry Y.
 * Date: December 19, 2018
 * Desc: This script makes critters turn around if they are about to run into something. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterCollisionDetect : MonoBehaviour {

	// The direction the gameobject/collider is facing relative to the parent critter object.
	[System.Serializable] public enum Direction {Front, Left, Right}

	public bool isAfraidOfPlayers;
	public bool isAfraidOfMonsters;
	public Direction colliderDirection;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.tag != "Player" || isAfraidOfPlayers) && (collision.tag != "Monster" || isAfraidOfMonsters) && 
			 collision.tag != "PlayerOrVillainChild" && collision.tag != "IgnoredByCritters")
		{
			string myName = GetComponentInParent<CritterMovement>().critterName;
			Debug.Log(myName + " is turning around.");
			if (collision.tag == "Player")
			{
				Debug.Log(myName + " was spooked by a player.");
			}
			else if (collision.tag == "Monster")
			{
				Debug.Log(myName + " was spooked by a monster.");
			}

			if (colliderDirection == Direction.Front) // Do a 180 degree turn
			{
				GetComponentInParent<CritterMovement>().TurnAround(180);
			}
			else if (colliderDirection == Direction.Left) // Go right
			{
				GetComponentInParent<CritterMovement>().TurnAround(-60, true, 30);
			}
			else if (colliderDirection == Direction.Right) // Go left
			{
				GetComponentInParent<CritterMovement>().TurnAround(60, true, 30);
			}
		}
	}
}
