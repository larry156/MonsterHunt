/* Name: Larry Y.
 * Date: December 10, 2018
 * Desc: This script should be attached to a StunManager GameObject which is a child of the player.
 *		 If another player triggers the collider, this player should be able to stun them. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStun : NetworkBehaviour
{
	private OnlineMovement parentScript;
	private bool actionTaken, uiBtnPressed;

	private void Start()
	{
		parentScript = GetComponentInParent<OnlineMovement>();
	}

	private void FixedUpdate()
	{
		actionTaken = false;
	}

	// If player presses the stun key while another player is nearby
	private void OnTriggerStay2D(Collider2D other)
	{
		// So the log isn't spammed with "Stun is still on cooldown".
		if (!actionTaken)
		{
			// Only stun if this player is pressing the button, the other object is a player, this player isn't stunned,
			// this object's parent is the local player, the ability is not on cooldown, and the other player is not already stunned.
			if ((Input.GetButtonDown("Stun") || uiBtnPressed) &&
				other.tag == "Player" &&
				!parentScript.GetComponent<OnlineMovement>().isStunned &&
				parentScript.IsControlledPlayer() &&
				!other.GetComponent<OnlineMovement>().IsControlledPlayer() &&
				Time.time - parentScript.CheckLastStun() > parentScript.stunCooldown &&
				!other.GetComponent<OnlineMovement>().isStunned)
			{
				uiBtnPressed = false;
				Debug.Log("You did the stun");
				Debug.Log(other.name);
				parentScript.SetLastStun(Time.time);
				parentScript.CallStunFunc(other.gameObject);
				actionTaken = true;

				// Tell the UI that a stun happened.
				parentScript.btnStun.GetComponent<UIAbilityButtons>().DoAction();
			}
			else if ((Input.GetButtonDown("Stun") || uiBtnPressed) && Time.time - parentScript.CheckLastStun() < parentScript.stunCooldown)
			{
				uiBtnPressed = false;
				Debug.Log("Stun is still on cooldown");
				actionTaken = true;
			}
		}
	}

	// This function is called when the player presses the stun button on their HUD/UI.
	public void ButtonStun()
	{
		if (!uiBtnPressed)
		{
			uiBtnPressed = true;
		}
	}
}
