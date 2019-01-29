/* Name: Larry Y.
 * Date: January 14, 2019
 * Desc: This script handles UI buttons for players' abilities. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityButtons : MonoBehaviour {

	private Button myButton; // This script should be attached directly to a button.
	private OnlineMovement parentScript;
	private float abilityCooldown, lastAbilityTime;
	private bool firstActionDone = false;

	public string trackedAbility;

	// Use this for initialization
	private void Start () {
		myButton = GetComponent<Button>();
		parentScript = GetComponentInParent<OnlineMovement>(); // There should only be one OnlineMovement in all parents: our player's.

		// Set the proper cooldowns
		if (trackedAbility == "Stun")
		{
			abilityCooldown = parentScript.stunCooldown;
		}
		else if (trackedAbility == "Dash")
		{
			abilityCooldown = parentScript.dashCooldown;
		}
	}

	private void FixedUpdate()
	{
		// Get the time at which the user last used their ability
		if (trackedAbility == "Stun")
		{
			lastAbilityTime = parentScript.CheckLastStun();
		}
		else if (trackedAbility == "Dash")
		{
			lastAbilityTime = parentScript.CheckLastDash();
		}

		// Disable the button and change the text to a timer.
		// firstActionDone is true as soon an ability is used.
		if (firstActionDone && Time.time - lastAbilityTime < abilityCooldown)
		{
			float nextAbilityTime = lastAbilityTime + abilityCooldown; // When is the user allowed to use the ability again.
			myButton.interactable = false;
			myButton.GetComponentInChildren<Text>().text = (nextAbilityTime - Time.time).ToString("F2") + "s";
		}
		// Re-enable the button and restore the text.
		else
		{
			myButton.interactable = true;
			myButton.GetComponentInChildren<Text>().text = trackedAbility;
		}
	}

	// Used by OnlineMovement and PlayerStun to tell this script that an action has been done, so it should start updating the timers.
	public void DoAction()
	{
		Debug.Log("Going to do a " + trackedAbility);
		firstActionDone = true;
	}

	// Call the appropriate abilities. These are called from the inspector.
	public void Stun()
	{
		Debug.Log("Stun pressed.");
		//firstActionDone = true;
		parentScript.GetComponentInChildren<PlayerStun>().ButtonStun();
	}
	public void Dash()
	{
		Debug.Log("Dash pressed.");
		//firstActionDone = true;
		parentScript.Dash();
	}
}
